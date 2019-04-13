using System;
using articledata.core.Models;
using articledata.domain.WebPages;
using articledata.domain.WebPages.Cards;
using HtmlAgilityPack;

namespace articledata.infrastructure.WebPages.Cards
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
            var yugiohCard = new YugiohCard();

            yugiohCard.ImageUrl = _cardHtmlDocument.ImageUrl(htmlDocument);
            yugiohCard.Name = _cardHtmlDocument.Name(htmlDocument);
            yugiohCard.Description = _cardHtmlDocument.Description(htmlDocument);
            yugiohCard.CardNumber = _cardHtmlDocument.CardNumber(htmlDocument);
            yugiohCard.CardType = _cardHtmlDocument.CardType(htmlDocument);
            yugiohCard.Property = _cardHtmlDocument.Property(htmlDocument);
            yugiohCard.Attribute = _cardHtmlDocument.Attribute(htmlDocument);
            yugiohCard.Level = _cardHtmlDocument.Level(htmlDocument);
            yugiohCard.Rank = _cardHtmlDocument.Rank(htmlDocument);
            yugiohCard.AtkDef = _cardHtmlDocument.AtkDef(htmlDocument);
            yugiohCard.AtkLink = _cardHtmlDocument.AtkLink(htmlDocument);
            yugiohCard.Types = _cardHtmlDocument.Types(htmlDocument);
            yugiohCard.Materials = _cardHtmlDocument.Materials(htmlDocument);
            yugiohCard.CardEffectTypes = _cardHtmlDocument.CardEffectTypes(htmlDocument);
            yugiohCard.PendulumScale = _cardHtmlDocument.PendulumScale(htmlDocument);

            return yugiohCard;
        }

    }
}