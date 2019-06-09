using System;
using HtmlAgilityPack;

namespace semanticsearch.domain.WebPage
{
    public interface ISemanticSearchResultsWebPage
    {
        void Load(string url);
        HtmlNodeCollection TableRows { get; }
        bool HasNextPage { get; }
        HtmlNode NextPage { get; }
        Uri CurrentWebPageUri { get; }
        string NextPageLink();
    }
}