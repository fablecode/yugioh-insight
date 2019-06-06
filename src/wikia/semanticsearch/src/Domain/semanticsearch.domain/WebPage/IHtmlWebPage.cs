using System;
using HtmlAgilityPack;

namespace semanticsearch.domain.WebPage
{
    public interface IHtmlWebPage
    {
        HtmlDocument Load(string url);

        HtmlDocument Load(Uri url);

    }
}