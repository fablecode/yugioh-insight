using System;
using HtmlAgilityPack;

namespace carddata.domain.WebPages
{
    public interface IHtmlWebPage
    {
        HtmlDocument Load(string webPageUrl);
        HtmlDocument Load(Uri webPageUrl);
    }
}