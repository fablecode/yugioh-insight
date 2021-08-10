using System;
using carddata.core.Models;
using carddata.domain.WebPages;
using carddata.domain.WebPages.Cards;
using HtmlAgilityPack;

namespace carddata.infrastructure.WebPages.Cards
{
    public class CardWebPage : ICardWebPage
    {
        private readonly ICardHtmlDocument _cardHtmlDocument;
        private readonly IHtmlWebPage _htmlWebPage;

        public CardWebPage(ICardHtmlDocument cardHtmlDocument, IHtmlWebPage htmlWebPage)
        {
            _cardHtmlDocument = cardHtmlDocument;
            _htmlWebPage = htmlWebPage;
        }

        public ArticleProcessed GetYugiohCard(Article article)
        {
            var card = GetYugiohCard(article.Url);

            return new ArticleProcessed {Article = article, Card = card};
        }

        public YugiohCard GetYugiohCard(string url)
        {
            return GetYugiohCard(new Uri(url));
        }

        public YugiohCard GetYugiohCard(Uri url)
        {
            var htmlDocument = _htmlWebPage.Load(url);
            return GetYugiohCard(htmlDocument);
        }

        public YugiohCard GetYugiohCard(HtmlDocument htmlDocument)
        {
            var yugiohCard = new YugiohCard
            {
                ImageUrl = _cardHtmlDocument.ImageUrl(htmlDocument),
                Name = _cardHtmlDocument.Name(htmlDocument),
                Description = _cardHtmlDocument.Description(htmlDocument),
                CardNumber = _cardHtmlDocument.CardNumber(htmlDocument),
                CardType = _cardHtmlDocument.CardType(htmlDocument),
                Property = _cardHtmlDocument.Property(htmlDocument),
                Attribute = _cardHtmlDocument.Attribute(htmlDocument),
                Level = _cardHtmlDocument.Level(htmlDocument),
                Rank = _cardHtmlDocument.Rank(htmlDocument),
                AtkDef = _cardHtmlDocument.AtkDef(htmlDocument),
                AtkLink = _cardHtmlDocument.AtkLink(htmlDocument),
                Types = _cardHtmlDocument.Types(htmlDocument),
                Materials = _cardHtmlDocument.Materials(htmlDocument),
                CardEffectTypes = _cardHtmlDocument.CardEffectTypes(htmlDocument),
                PendulumScale = _cardHtmlDocument.PendulumScale(htmlDocument)
            };

            return yugiohCard;
        }

    }
}
