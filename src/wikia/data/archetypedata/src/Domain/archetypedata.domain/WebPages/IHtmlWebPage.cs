using System;
using HtmlAgilityPack;

namespace archetypedata.domain.WebPages
{
    public interface IHtmlWebPage
    {
        HtmlDocument Load(string url);

        HtmlDocument Load(Uri url);

    }
}