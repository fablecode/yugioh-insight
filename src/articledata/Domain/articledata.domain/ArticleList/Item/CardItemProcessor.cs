using articledata.core.ArticleList.Processor;
using articledata.core.Constants;
using articledata.core.Exceptions;
using articledata.core.Models;
using articledata.domain.Services.Messaging.Cards;
using System;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.ArticleList.Item
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