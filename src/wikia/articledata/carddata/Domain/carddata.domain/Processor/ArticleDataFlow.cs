using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using carddata.core.Models;
using carddata.core.Processor;
using carddata.domain.Services.Messaging;
using carddata.domain.WebPages.Cards;

namespace carddata.domain.Processor
{
    public class ArticleDataFlow : IArticleDataFlow
    {
        private readonly ICardWebPage _cardWebPage;
        private readonly IYugiohCardQueue _yugiohCardQueue;
        private readonly BufferBlock<Article> _articleBufferBlock;
        private readonly TransformBlock<Article, YugiohCard> _yugiohDataTransformBlock;
        private ActionBlock<YugiohCard> _publishToQueueActionBlock;
        private readonly ConcurrentDictionary<long, TaskCompletionSource<ArticleCompletion>> _jobs;

        public ArticleDataFlow(ICardWebPage cardWebPage, IYugiohCardQueue yugiohCardQueue)
        {
            _cardWebPage = cardWebPage;
            _yugiohCardQueue = yugiohCardQueue;

            _jobs = new ConcurrentDictionary<long, TaskCompletionSource<ArticleCompletion>>();

            // Dataflow options
            var nonGreedy = new ExecutionDataflowBlockOptions { BoundedCapacity = Environment.ProcessorCount, MaxDegreeOfParallelism = Environment.ProcessorCount };
            var flowComplete = new DataflowLinkOptions { PropagateCompletion = true };

            // Pipeline members
            var processorCount = Environment.ProcessorCount;

            _articleBufferBlock = new BufferBlock<Article>();
            _yugiohDataTransformBlock = new TransformBlock<Article, YugiohCard>(article => _cardWebPage.GetYugiohCard(new Uri(article.Url)), nonGreedy);
            _publishToQueueActionBlock = new ActionBlock<YugiohCard>(async yugiohCard =>
                {
                    await _yugiohCardQueue.Publish(yugiohCard);
                },
                // Specify a maximum degree of parallelism.
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = processorCount
                });

            // Form the pipeline
            _articleBufferBlock.LinkTo(_yugiohDataTransformBlock, flowComplete);
            _yugiohDataTransformBlock.LinkTo(_publishToQueueActionBlock, flowComplete);

            // Mark the head of the pipeline as complete. The continuation tasks  
            // propagate completion through the pipeline as each part of the  
            // pipeline finishes.
            _publishToQueueActionBlock.Completion.Wait();
        }

        public async Task<ArticleCompletion> ProcessDataFlow(Article article)
        {
            var taggedData = TagInputData(article);
            var job = CreateJob(taggedData);
            var isJobAdded = _jobs.TryAdd(job.Key, job.Value);

            if (isJobAdded)
                await _articleBufferBlock.SendAsync(article);
            else
            {
                job.Value.SetResult(new ArticleCompletion { Message = article, Exception = new Exception($"Job not created for item with id {article.Id}") });
            }

            return await job.Value.Task;
        }

        #region private helper

        private static KeyValuePair<long, Article> TagInputData(Article data)
        {
            return new KeyValuePair<long, Article>(data.Id, data);
        }

        private static KeyValuePair<long, TaskCompletionSource<ArticleCompletion>> CreateJob(KeyValuePair<long, Article> taggedData)
        {
            var id = taggedData.Key;
            var jobCompletionSource = new TaskCompletionSource<ArticleCompletion>();
            return new KeyValuePair<long, TaskCompletionSource<ArticleCompletion>>(id, jobCompletionSource);
        }

        #endregion
    }
}