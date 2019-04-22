using System;
using articledata.core.Models;
using HtmlAgilityPack;

namespace articledata.domain.WebPages.Cards
{
    public interface ICardWebPage
    {
        YugiohCard GetYugiohCard(string url);
        YugiohCard GetYugiohCard(Uri url);
        YugiohCard GetYugiohCard(HtmlDocument htmlDocument);
    }
}