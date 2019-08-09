using cardsectiondata.application.Configuration;
using cardsectiondata.core.Models;
using cardsectiondata.domain.WebPages;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace cardsectiondata.infrastructure.WebPages
{
    public class TipRelatedWebPage : ITipRelatedWebPage
    {
        private readonly IOptions<AppSettings> _appsettingsOptions;
        private readonly ITipRelatedCardList _tipRelatedCardList;
        private readonly ITipRelatedHtmlDocument _tipRelatedHtmlDocument;
        private readonly ISemanticSearch _semanticSearch;

        public TipRelatedWebPage
        (
            IOptions<AppSettings> appsettingsOptions,
            ITipRelatedCardList tipRelatedCardList,
            ITipRelatedHtmlDocument tipRelatedHtmlDocument,
            ISemanticSearch semanticSearch
        )
        {
            _appsettingsOptions = appsettingsOptions;
            _tipRelatedCardList = tipRelatedCardList;
            _tipRelatedHtmlDocument = tipRelatedHtmlDocument;
            _semanticSearch = semanticSearch;
        }

        public void GetTipRelatedCards(CardSection section, Article item)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var tipWebPage = new HtmlWeb().Load(_appsettingsOptions.Value.WikiaDomainUrl + item.Url);

            //Get tip related card list url
            var cardListUrl = _tipRelatedHtmlDocument.GetUrl(tipWebPage);

            //get tips related card list table
            var cardListTable = _tipRelatedHtmlDocument.GetTable(tipWebPage);

            GetTipRelatedCards(section, cardListUrl, cardListTable);
        }

        public void GetTipRelatedCards(CardSection section, string tipRelatedCardListUrl, HtmlNode tipRelatedCardListTable)
        {
            if(section == null)
                throw new ArgumentNullException(nameof(section));

            if (!string.IsNullOrEmpty(tipRelatedCardListUrl))
            {
                var cardsFromUrl = _semanticSearch.CardsByUrl(tipRelatedCardListUrl);
                section.ContentList.AddRange(cardsFromUrl.Select(c => c.Name));
            }
            else if (tipRelatedCardListTable != null)
            {
                var cardsFromTable = _tipRelatedCardList.ExtractCardsFromTable(tipRelatedCardListTable);
                section.ContentList.AddRange(cardsFromTable);
            }
        }
    }
}