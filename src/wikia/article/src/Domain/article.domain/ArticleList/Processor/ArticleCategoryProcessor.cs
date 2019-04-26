using article.core.ArticleList.DataSource;
using article.core.ArticleList.Processor;
using article.core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.ArticleList.Processor
{
    public class ArticleCategoryProcessor : IArticleCategoryProcessor
    {
        private readonly IArticleBatchProcessor _articleBatchProcessor;
        private readonly IArticleCategoryDataSource _articleCategoryDataSource;

        public ArticleCategoryProcessor(IArticleCategoryDataSource articleCategoryDataSource, IArticleBatchProcessor articleBatchProcessor)
        {
            _articleCategoryDataSource = articleCategoryDataSource;
            _articleBatchProcessor = articleBatchProcessor;
        }

        public async Task<IEnumerable<ArticleBatchTaskResult>> Process(IEnumerable<string> categories, int pageSize)
        {
            var results = new List<ArticleBatchTaskResult>();

            foreach (var category in categories)
            {
                results.Add(await Process(category, pageSize));
            }

            return results;
        }

        public async Task<ArticleBatchTaskResult> Process(string category, int pageSize)
        {
            var response = new ArticleBatchTaskResult { Category = category };

            // Data flow options
            var processorCount = Environment.ProcessorCount / 2;

            var nonGreedy = new ExecutionDataflowBlockOptions { BoundedCapacity = processorCount, MaxDegreeOfParallelism = processorCount };
            var flowComplete = new DataflowLinkOptions { PropagateCompletion = true };

            // Pipeline members
            var articleBatchBufferBlock = new BufferBlock<UnexpandedArticle[]>();
            var articleTransformBlock = new TransformBlock<UnexpandedArticle[], ArticleBatchTaskResult>(articles => _articleBatchProcessor.Process(category, articles), nonGreedy);
            var articleActionBlock = new ActionBlock<ArticleBatchTaskResult>(articleBatchTaskResult => FinishedProcessing(response, articleBatchTaskResult));

            // Form the pipeline
            articleBatchBufferBlock.LinkTo(articleTransformBlock, flowComplete);
            articleTransformBlock.LinkTo(articleActionBlock, flowComplete);

            // Process "Category" and generate article batch data
           await _articleCategoryDataSource.Producer(category, pageSize, articleBatchBufferBlock);

            // Producer completed producing data. Signals to the "DataflowBlock" that it should not accept nor produce any more messages nor consume any more postponed messages.
            articleBatchBufferBlock.Complete();

            // Mark the head of the pipeline as complete. The continuation tasks  
            // propagate completion through the pipeline as each part of the  
            // pipeline finishes.
            await articleActionBlock.Completion;

            return response;
        }

        private static void FinishedProcessing(ArticleBatchTaskResult response, ArticleBatchTaskResult articleBatchTaskResult)
        {
            response.Processed += articleBatchTaskResult.Processed;
            response.Failed.AddRange(articleBatchTaskResult.Failed);
        }
    }
}