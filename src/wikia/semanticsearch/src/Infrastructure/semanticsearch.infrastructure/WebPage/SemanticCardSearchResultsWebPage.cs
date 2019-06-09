using System;
using HtmlAgilityPack;
using semanticsearch.domain.WebPage;

namespace semanticsearch.infrastructure.WebPage
{
    public class SemanticCardSearchResultsWebPage : ISemanticCardSearchResultsWebPage
    {
        public string Name(HtmlNode tableRow)
        {
            return tableRow.SelectSingleNode("td[position() = 1]")?.InnerText.Trim();
        }
        public string Url(HtmlNode tableRow, Uri uri)
        {
            var cardUrl = tableRow.SelectSingleNode("td[position() = 1]/a")?.Attributes["href"]?.Value;

            return !string.IsNullOrWhiteSpace(cardUrl) ? $"{uri.GetLeftPart(UriPartial.Authority)}{cardUrl}" : null;
        }
    }
}