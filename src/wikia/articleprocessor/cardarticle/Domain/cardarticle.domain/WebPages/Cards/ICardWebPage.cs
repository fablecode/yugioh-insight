using System;
using cardarticle.core.Models;
using HtmlAgilityPack;

namespace cardarticle.domain.WebPages.Cards
{
    public interface ICardWebPage
    {
        YugiohCard GetYugiohCard(string url);
        YugiohCard GetYugiohCard(Uri url);
        YugiohCard GetYugiohCard(HtmlDocument htmlDocument);
    }
}