using System;
using System.Net;
using HtmlAgilityPack;
using semanticsearch.domain.WebPage;

namespace semanticsearch.infrastructure.WebPage
{
    public class SemanticSearchResultsWebPage : ISemanticSearchResultsWebPage
    {
        private readonly IHtmlWebPage _htmlWebPage;
        private HtmlDocument _currentWebPage;
        public Uri CurrentWebPageUri { get; private set; }

        public SemanticSearchResultsWebPage(IHtmlWebPage htmlWebPage)
        {
            _htmlWebPage = htmlWebPage;
        }

        public void Load(string url)
        {
            CurrentWebPageUri = new Uri(url);
            _currentWebPage = _htmlWebPage.Load(url);
        }

        public HtmlNodeCollection TableRows =>
            _currentWebPage
                .DocumentNode
                .SelectNodes("//table[contains(@class, 'sortable wikitable smwtable')]/tbody/tr") ?? _currentWebPage.DocumentNode.SelectNodes("//table[contains(@class, 'sortable wikitable smwtable card-list')]/tbody/tr");

        public bool HasNextPage => NextPage != null;

        public HtmlNode NextPage => _currentWebPage.DocumentNode.SelectSingleNode("//a[contains(text(), 'next') and contains(@title, 'Next 500 results')]");
        public string NextPageLink()
        {
            var hrefLink = $"{CurrentWebPageUri.GetLeftPart(UriPartial.Authority)}{NextPage.Attributes["href"].Value}";

            return WebUtility.HtmlDecode(hrefLink);
        }
    }
}