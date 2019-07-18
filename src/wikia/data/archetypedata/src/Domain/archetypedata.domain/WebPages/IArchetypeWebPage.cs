using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace archetypedata.domain.WebPages
{
    public interface IArchetypeWebPage
    {
        IEnumerable<string> Cards(Uri archetypeUrl);
        string GetFurtherResultsUrl(HtmlDocument archetypeWebPage);
        List<string> CardsFromFurtherResultsUrl(string furtherResultsUrl);
        Task<string> ArchetypeThumbnail(long articleId, string url);
    }
}