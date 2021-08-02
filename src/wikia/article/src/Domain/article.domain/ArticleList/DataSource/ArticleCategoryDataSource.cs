using article.core.ArticleList.DataSource;
using System;
using System.Collections.Generic;
using wikia.Api;
using wikia.Models.Article;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.ArticleList.DataSource
{
    public class ArticleCategoryDataSource : IArticleCategoryDataSource
    {
        private readonly IWikiArticleList _articleList;

        public ArticleCategoryDataSource(IWikiArticleList articleList)
        {
            _articleList = articleList;
        }
        public async IAsyncEnumerable<UnexpandedArticle[]> Producer(string category, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Article Category cannot be null or empty", nameof(category));

            var nextBatch = await _articleList.AlphabeticalList(new ArticleListRequestParameters { Category = category, Limit = pageSize });

            bool isNextBatchAvailable;

            yield return nextBatch.Items;

            do
            {
                isNextBatchAvailable = !string.IsNullOrEmpty(nextBatch.Offset);

                if (isNextBatchAvailable)
                {
                    nextBatch = await _articleList.AlphabeticalList(new ArticleListRequestParameters
                    {
                        Category = category,
                        Limit = pageSize,
                        Offset = nextBatch.Offset
                    });

                    yield return nextBatch.Items;
                }

            } while (isNextBatchAvailable);
        }
    }
}