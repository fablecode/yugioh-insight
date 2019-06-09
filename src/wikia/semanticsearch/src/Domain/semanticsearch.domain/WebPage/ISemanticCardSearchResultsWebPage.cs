using System;
using HtmlAgilityPack;

namespace semanticsearch.domain.WebPage
{
    public interface ISemanticCardSearchResultsWebPage
    {
        string Name(HtmlNode tableRow);
        string Url(HtmlNode tableRow, Uri uri);
    }
}