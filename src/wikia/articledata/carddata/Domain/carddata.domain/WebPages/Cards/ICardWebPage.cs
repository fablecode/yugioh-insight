using System;
using carddata.core.Models;
using HtmlAgilityPack;

namespace carddata.domain.WebPages.Cards
{
    public interface ICardWebPage
    {
        YugiohCard GetYugiohCard(string url);
        YugiohCard GetYugiohCard(Uri url);
        ArticleProcessed GetYugiohCard(Article article);
        YugiohCard GetYugiohCard(HtmlDocument htmlDocument);
    }
}