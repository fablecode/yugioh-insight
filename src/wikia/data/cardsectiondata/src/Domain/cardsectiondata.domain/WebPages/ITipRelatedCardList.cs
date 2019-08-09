using System.Collections.Generic;
using HtmlAgilityPack;

namespace cardsectiondata.domain.WebPages
{
    public interface ITipRelatedCardList
    {
        List<string> ExtractCardsFromTable(HtmlNode table);
    }
}