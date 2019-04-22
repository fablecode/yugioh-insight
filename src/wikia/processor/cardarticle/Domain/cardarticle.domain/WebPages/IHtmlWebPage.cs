using System;
using HtmlAgilityPack;

namespace cardarticle.domain.WebPages
{
    public interface IHtmlWebPage
    {
        HtmlDocument Load(string webPageUrl);
        HtmlDocument Load(Uri webPageUrl);
    }
}