using System;
using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using article.core.Constants;
using article.core.Exceptions;
using article.core.Models;
using article.domain.Services.Messaging;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.ArticleList.Item
{
    public class ArticleItemProcessor : IArticleItemProcessor
    {
        private readonly IQueue<Article> _queue;

        public ArticleItemProcessor(IQueue<Article> queue)
        {
            _queue = queue;
        }
        public async Task<ArticleTaskResult> ProcessItem(UnexpandedArticle item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var articleTaskResult = new ArticleTaskResult { Article = item };

            try
            {
                await _queue.Publish(item);

                articleTaskResult.IsSuccessfullyProcessed = true;
            }
            catch (Exception ex)
            {
                articleTaskResult.Failed = new ArticleException { Article = item, Exception = ex };
            }

            return articleTaskResult;
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardTips  ||
                   category == ArticleCategory.CardRulings ||
                   category == ArticleCategory.CardTrivia;
        }
    }
}