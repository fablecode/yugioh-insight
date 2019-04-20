using System;
using carddata.core.Models;
using HtmlAgilityPack;

namespace carddata.domain.WebPages.Cards
{
    public interface ICardWebPage
    {
        YugiohCard GetYugiohCard(string url);
        YugiohCard GetYugiohCard(Uri url);
        YugiohCard GetYugiohCard(HtmlDocument htmlDocument);
    }
}