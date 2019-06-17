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
        private readonly ConcurrentDictionary<Guid, TaskCompletionSource<ArticleCompletion>> _jobs;

        public ArticleDataFlow(ICardWebPage cardWebPage, IYugiohCardQueue yugiohCardQueue)
        {
            _jobs = new ConcurrentDictionary<Guid, TaskCompletionSource<ArticleCompletion>>();

            // Data flow options
            var maxDegreeOfParallelism = Environment.ProcessorCount;
            var nonGreedy = new ExecutionDataflowBlockOptions { BoundedCapacity = maxDegreeOfParallelism, MaxDegreeOfParallelism = maxDegreeOfParallelism };

            // Pipeline members
            _articleBufferBlock = new BufferBlock<Article>();
            var yugiohDataTransformBlock = new TransformBlock<Article, ArticleProcessed>(article => cardWebPage.GetYugiohCard(article), nonGreedy);
            var yugiohCardPublishTransformBlock = new TransformBlock<ArticleProcessed, YugiohCardCompletion>(articleProcessed => yugiohCardQueue.Publish(articleProcessed), nonGreedy);
            var publishToQueueActionBlock = new ActionBlock<YugiohCardCompletion>(yugiohCardCompletion => FinishedProcessing(yugiohCardCompletion));

            // Form the pipeline
            _articleBufferBlock.LinkTo(yugiohDataTransformBlock);
            yugiohDataTransformBlock.LinkTo(yugiohCardPublishTransformBlock);
            yugiohCardPublishTransformBlock.LinkTo(publishToQueueActionBlock);
        }


        public async Task<ArticleCompletion> ProcessDataFlow(Article article)
        {
            var taggedData = TagInputData(article);
            var (jobId, taskCompletionSource) = CreateJob(taggedData);
            var isJobAdded = _jobs.TryAdd(jobId, taskCompletionSource);

            if (isJobAdded)
                await _articleBufferBlock.SendAsync(article);
            else
            {
                taskCompletionSource.SetResult(new ArticleCompletion { Message = article, Exception = new Exception($"Job not created for item with id {article.Id}") });
            }

            return await taskCompletionSource.Task;
        }

        #region private helper

        private static KeyValuePair<Guid, Article> TagInputData(Article data)
        {
            return new KeyValuePair<Guid, Article>(data.CorrelationId, data);
        }

        private static KeyValuePair<Guid, TaskCompletionSource<ArticleCompletion>> CreateJob(KeyValuePair<Guid, Article> taggedData)
        {
            var id = taggedData.Key;
            var jobCompletionSource = new TaskCompletionSource<ArticleCompletion>();
            return new KeyValuePair<Guid, TaskCompletionSource<ArticleCompletion>>(id, jobCompletionSource);
        }

        private void FinishedProcessing(YugiohCardCompletion yugiohCardCompletion)
        {
            _jobs.TryRemove(yugiohCardCompletion.Article.CorrelationId, out var job);

            if (yugiohCardCompletion.IsSuccessful)
            {
                job.SetResult(new ArticleCompletion { IsSuccessful = true, Message = yugiohCardCompletion.Article });
            }
            else
            {
                var exceptionMessage = $"Card Article with id '{yugiohCardCompletion.Article.Id}' and correlation id {yugiohCardCompletion.Article.CorrelationId} not processed.";
                job.SetException(new Exception(exceptionMessage));
            }
        }


        #endregion
    }
}