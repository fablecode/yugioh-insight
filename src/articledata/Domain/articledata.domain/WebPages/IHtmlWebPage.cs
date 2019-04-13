using System;
using HtmlAgilityPack;

namespace articledata.domain.WebPages
{
    public interface IHtmlWebPage
    {
        HtmlDocument Load(string webPageUrl);
        HtmlDocument Load(Uri webPageUrl);
    }
}