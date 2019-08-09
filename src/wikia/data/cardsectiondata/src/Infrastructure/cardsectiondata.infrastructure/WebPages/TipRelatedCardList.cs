using System.Collections.Generic;
using System.Linq;
using cardsectiondata.domain.WebPages;
using HtmlAgilityPack;

namespace cardsectiondata.infrastructure.WebPages
{
    public class TipRelatedCardList : ITipRelatedCardList
    {
        public List<string> ExtractCardsFromTable(HtmlNode table)
        {
            var cardNameList = table.SelectNodes("//tr/td[position() = 1]/a");

            return cardNameList.Select(cn => cn.InnerText).ToList();
        }
    }
}