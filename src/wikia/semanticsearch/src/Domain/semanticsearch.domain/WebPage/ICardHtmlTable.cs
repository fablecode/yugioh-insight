using System.Collections.Generic;
using HtmlAgilityPack;

namespace semanticsearch.domain.WebPage
{
    public interface ICardHtmlTable
    {
        string GetValue(string key, HtmlNode htmlTable);
        Dictionary<string, string> ProfileData(HtmlNode htmlTable);
        string GetValue(string[] keys, HtmlNode htmlTable);
    }
}