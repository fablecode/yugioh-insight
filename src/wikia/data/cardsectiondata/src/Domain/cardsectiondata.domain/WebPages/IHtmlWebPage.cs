using System;
using HtmlAgilityPack;

namespace cardsectiondata.domain.WebPages
{
    public interface IHtmlWebPage
    {
        HtmlDocument Load(string url);

        HtmlDocument Load(Uri url);

    }
}