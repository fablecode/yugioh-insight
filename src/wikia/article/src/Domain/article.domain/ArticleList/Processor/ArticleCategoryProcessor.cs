using article.core.ArticleList.DataSource;
using article.core.ArticleList.Processor;
using article.core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            await foreach (var unexpandedArticleBatch in _articleCategoryDataSource.Producer(category, pageSize))
            {
                var articleBatchTaskResult = await _articleBatchProcessor.Process(category, unexpandedArticleBatch);
                response.Processed += articleBatchTaskResult.Processed;
                response.Failed.AddRange(articleBatchTaskResult.Failed);
            }

            return response;
        }
    }
}