using System;
using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using article.core.Constants;
using article.core.Exceptions;
using article.core.Models;
using article.domain.Services.Messaging.Cards;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.ArticleList.Item
{
    public class CardItemProcessor : IArticleItemProcessor
    {
        private readonly ICardArticleQueue _cardArticleQueue;

        public CardItemProcessor(ICardArticleQueue cardArticleQueue)
        {
            _cardArticleQueue = cardArticleQueue;
        }
        public async Task<ArticleTaskResult> ProcessItem(UnexpandedArticle item)
        {
            var articleTaskResult = new ArticleTaskResult { Article = item };

            try
            {
                await _cardArticleQueue.Publish(item);

                articleTaskResult.IsSuccessfullyProcessed = true;
            }
            catch (Exception ex)
            {
                articleTaskResult.Failed = new ArticleException{ Article = item, Exception = ex};
            }

            return articleTaskResult;
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.TcgCards || category == ArticleCategory.OcgCards;
        }
    }
}