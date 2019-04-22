using System.Text.RegularExpressions;
using System.Threading;
using carddata.core.Helpers;
using carddata.domain.WebPages.Cards;
using HtmlAgilityPack;

namespace carddata.infrastructure.WebPages.Cards
{
    public class CardHtmlDocument : ICardHtmlDocument
    {
        private readonly ICardHtmlTable _cardHtmlTable;

        public CardHtmlDocument(ICardHtmlTable cardHtmlTable)
        {
            _cardHtmlTable = cardHtmlTable;
        }

        public string ImageUrl(HtmlDocument htmlDocument)
        {
            var imageUrl = htmlDocument.DocumentNode.SelectSingleNode("//td[@class='cardtable-cardimage']/a/img").Attributes["src"].Value;

            return ImageHelper.ExtractImageUrl(imageUrl);
        }

        public string Name(HtmlDocument htmlDocument)
        {
            var htmlTableNode = ExtractHtmlCardTableNode(htmlDocument);

            return _cardHtmlTable.GetValue(CardHtmlTable.Name, htmlTableNode);
        }

        public string Description(HtmlDocument htmlDocument)
        {
            var pattern = @"(?!</?br>)<.*?>";
            var descNode = htmlDocument.DocumentNode.SelectSingleNode("//b[text()[contains(., 'Card descriptions')]]/../table[1]/tr[1]/td/table/tr[3]/td")?.InnerHtml;

            if (descNode != null)
            {
                descNode = Regex.Replace(descNode, pattern, string.Empty, RegexOptions.Multiline);

                return descNode.Trim();
            }

            return string.Empty;
        }

        public string CardNumber(HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(CardHtmlTable.Number, htmlDocument);
        }

        public string CardType(HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(CardHtmlTable.CardType, htmlDocument);
        }

        public string Property(HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(CardHtmlTable.Property, htmlDocument);
        }

        public string Attribute(HtmlDocument htmlDocument)
        {
            var attribute = ExtractHtmlCardTableValue(CardHtmlTable.Attribute, htmlDocument);

            if (string.IsNullOrWhiteSpace(attribute))
                return string.Empty;

            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;

            return textInfo.ToTitleCase(attribute.Trim().ToLower());

        }

        public int? Level(HtmlDocument htmlDocument)
        {
            var level = ExtractHtmlCardTableValue(CardHtmlTable.Level, htmlDocument);
            return string.IsNullOrWhiteSpace(level) ? null : (int?)int.Parse(level);
        }

        public int? Rank(HtmlDocument htmlDocument)
        {
            var rank = ExtractHtmlCardTableValue(CardHtmlTable.Rank, htmlDocument);
            return string.IsNullOrWhiteSpace(rank) ? null : (int?)int.Parse(rank);
        }

        public string AtkDef(HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(CardHtmlTable.AtkAndDef, htmlDocument);
        }

        public string AtkLink(HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(CardHtmlTable.AtkAndLink, htmlDocument);
        }

        public string LinkArrows(HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(CardHtmlTable.LinkArrows, htmlDocument);
        }

        public string Types(HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(CardHtmlTable.Types, htmlDocument);
        }

        public string Materials(HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(CardHtmlTable.Materials, htmlDocument);
        }

        public string CardEffectTypes(HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(CardHtmlTable.CardEffectTypes, htmlDocument);
        }

        public long? PendulumScale(HtmlDocument htmlDocument)
        {
            var scale = ExtractHtmlCardTableValue(CardHtmlTable.PendulumScale, htmlDocument);

            return string.IsNullOrWhiteSpace(scale) ? null : (long?)long.Parse(scale);
        }

        #region private helpers

        private static HtmlNode ExtractHtmlCardTableNode(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectSingleNode("//div[@id='WikiaArticle']//table[contains(@class, 'cardtable')]");
        }

        private string ExtractHtmlCardTableValue(string key, HtmlDocument htmlDocument)
        {
            return ExtractHtmlCardTableValue(new[] { key }, htmlDocument);
        }

        private string ExtractHtmlCardTableValue(string[] key, HtmlDocument htmlDocument)
        {
            var htmlTableNode = ExtractHtmlCardTableNode(htmlDocument);
            return _cardHtmlTable.GetValue(key, htmlTableNode);
        }

        #endregion
    }
}