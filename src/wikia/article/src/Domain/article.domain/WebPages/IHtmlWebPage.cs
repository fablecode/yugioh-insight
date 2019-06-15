using System;
using HtmlAgilityPack;

namespace article.domain.WebPages
{
    public interface IHtmlWebPage
    {
        HtmlDocument Load(string url);

        HtmlDocument Load(Uri url);

    }
}