using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using wikia.Api;
using wikia.Models.Article;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.ArticleList.Processor
{
    public class ArticleCategoryDataSource : IArticleCategoryDataSource
    {
        private readonly IWikiArticleList _articleList;

        public ArticleCategoryDataSource(IWikiArticleList articleList)
        {
            _articleList = articleList;
        }
        public async Task Producer(string category, int pageSize, ITargetBlock<UnexpandedArticle[]> targetBlock)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException(nameof(category));

            if (targetBlock == null)
                throw new ArgumentException(nameof(targetBlock));


            var nextBatch = await _articleList.AlphabeticalList(new ArticleListRequestParameters { Category = Uri.EscapeDataString(category), Limit = pageSize });

            bool isNextBatchAvailable;

            do
            {
                await targetBlock.SendAsync(nextBatch.Items);

                isNextBatchAvailable = !string.IsNullOrEmpty(nextBatch.Offset);

                if (isNextBatchAvailable)
                {
                    nextBatch = await _articleList.AlphabeticalList(new ArticleListRequestParameters
                    {
                        Category = category,
                        Limit = pageSize,
                        Offset = nextBatch.Offset
                    });
                }
            } while (isNextBatchAvailable);

            targetBlock.Complete();
        }
    }
}