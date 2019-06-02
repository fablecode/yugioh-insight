using carddata.core.Models;
using carddata.core.Processor;
using carddata.domain.Services.Messaging;
using carddata.domain.WebPages.Cards;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace carddata.domain.Processor
{
    public class ArticleDataFlow : IArticleDataFlow
    {
        private readonly BufferBlock<Article> _articleBufferBlock;
        private readonly ConcurrentDictionary<long, TaskCompletionSource<ArticleCompletion>> _jobs;
        private readonly TransformBlock<Article, ArticleProcessed> _yugiohDataTransformBlock;
        private readonly TransformBlock<ArticleProcessed, YugiohCardCompletion> _yugiohCardPublishTransformBlock;
        private readonly ActionBlock<YugiohCardCompletion> _publishToQueueActionBlock;

        public ArticleDataFlow(ICardWebPage cardWebPage, IYugiohCardQueue yugiohCardQueue)
        {
            _jobs = new ConcurrentDictionary<long, TaskCompletionSource<ArticleCompletion>>();

            // Data flow options
            var maxDegreeOfParallelism = Environment.ProcessorCount;
            var nonGreedy = new ExecutionDataflowBlockOptions { BoundedCapacity = maxDegreeOfParallelism, MaxDegreeOfParallelism = maxDegreeOfParallelism };

            // Pipeline members
            _articleBufferBlock = new BufferBlock<Article>();
            _yugiohDataTransformBlock = new TransformBlock<Article, ArticleProcessed>(article => cardWebPage.GetYugiohCard(article), nonGreedy);
            _yugiohCardPublishTransformBlock = new TransformBlock<ArticleProcessed, YugiohCardCompletion>(articleProcessed => yugiohCardQueue.Publish(articleProcessed), nonGreedy);
            _publishToQueueActionBlock = new ActionBlock<YugiohCardCompletion>(yugiohCardCompletion => FinishedProcessing(yugiohCardCompletion));

            // Form the pipeline
            _articleBufferBlock.LinkTo(_yugiohDataTransformBlock);
            _yugiohDataTransformBlock.LinkTo(_yugiohCardPublishTransformBlock);
            _yugiohCardPublishTransformBlock.LinkTo(_publishToQueueActionBlock);
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

        private void FinishedProcessing(YugiohCardCompletion yugiohCardCompletion)
        {
            _jobs.TryRemove(yugiohCardCompletion.Article.Id, out var job);

            if (yugiohCardCompletion.IsSuccessful)
            {
                job.SetResult(new ArticleCompletion { IsSuccessful = true, Message = yugiohCardCompletion.Article });
            }
            else
            {
                var exceptionMessage = $"Card Article with id '{yugiohCardCompletion.Article.Id}' not processed.";
                job.SetException(new Exception(exceptionMessage));
            }
        }


        #endregion
    }
}