using System;
using System.Threading.Tasks;
using articledata.core.Constants;
using articledata.domain.ArticleList.Processor;
using articledata.domain.Configuration;
using articledata.domain.mo;
using articledata.domain.WebPages.Cards;
using Microsoft.Extensions.Options;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.ArticleList.Item
{
    public class CardItemProcessor : IArticleItemProcessor
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly ICardWebPage _cardWebPage;

        public CardItemProcessor(IOptions<AppSettings> settings, ICardWebPage cardWebPage)
        {
            _settings = settings;
            _cardWebPage = cardWebPage;
        }
        public Task<ArticleTaskResult> ProcessItem(UnexpandedArticle item)
        {
            //var response = new ArticleTaskResult { Article = item };

            //var yugiohCard = _cardWebPage.GetYugiohCard(new Uri(new Uri(_settings.Value.WikiaDomainUrl), item.Url));

            //return response;

            throw new NotImplementedException();
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.TcgCards || category == ArticleCategory.OcgCards;
        }
    }
}