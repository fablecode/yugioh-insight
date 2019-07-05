using banlistdata.core.Models;
using banlistdata.core.Processor;
using banlistdata.domain.Services.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace banlistdata.domain.Processor
{
    public class ArticleDataFlow : IArticleDataFlow
    {
        private readonly BufferBlock<Article> _articleBufferBlock;
        private readonly ConcurrentDictionary<Guid, TaskCompletionSource<ArticleCompletion>> _jobs;

        public ArticleDataFlow(IBanlistProcessor banlistProcessor, IBanlistDataQueue banlistDataQueue)
        {
            _jobs = new ConcurrentDictionary<Guid, TaskCompletionSource<ArticleCompletion>>();

            // Data flow options
            var maxDegreeOfParallelism = Environment.ProcessorCount;
            var nonGreedy = new ExecutionDataflowBlockOptions { BoundedCapacity = maxDegreeOfParallelism, MaxDegreeOfParallelism = maxDegreeOfParallelism };

            // Pipeline members
            _articleBufferBlock = new BufferBlock<Article>();

            var banlistProcessorTransformBlock = new TransformBlock<Article, ArticleProcessed>(article => banlistProcessor.Process(article), nonGreedy);
            var publishBanlistTransformBlock = new TransformBlock<ArticleProcessed, YugiohBanlistCompletion>(articleProcessed => banlistDataQueue.Publish(articleProcessed), nonGreedy);
            var publishToQueueActionBlock = new ActionBlock<YugiohBanlistCompletion>(yugiohCardCompletion => FinishedProcessing(yugiohCardCompletion));

            // Form the pipeline
            _articleBufferBlock.LinkTo(banlistProcessorTransformBlock);
            banlistProcessorTransformBlock.LinkTo(publishBanlistTransformBlock);
            publishBanlistTransformBlock.LinkTo(publishToQueueActionBlock);
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

        private void FinishedProcessing(YugiohBanlistCompletion yugiohBanlistCompletion)
        {
            _jobs.TryRemove(yugiohBanlistCompletion.Article.CorrelationId, out var job);

            if (yugiohBanlistCompletion.IsSuccessful)
            {
                job.SetResult(new ArticleCompletion { IsSuccessful = true, Message = yugiohBanlistCompletion.Article });
            }
            else
            {
                var exceptionMessage = $"Card Article with id '{yugiohBanlistCompletion.Article.Id}' and correlation id {yugiohBanlistCompletion.Article.CorrelationId} not processed.";
                job.SetException(new Exception(exceptionMessage));
            }
        }


        #endregion
    }
}