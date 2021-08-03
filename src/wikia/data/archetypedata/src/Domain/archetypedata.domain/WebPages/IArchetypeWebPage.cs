using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using wikia.Models.Article.Details;

namespace archetypedata.domain.WebPages
{
    public interface IArchetypeWebPage
    {
        IEnumerable<string> Cards(Uri archetypeUrl);
        string GetFurtherResultsUrl(HtmlDocument archetypeWebPage);
        List<string> CardsFromFurtherResultsUrl(string furtherResultsUrl);
        Task<string> ArchetypeThumbnail(long articleId, string archetypeWebPageUrl);
        string ArchetypeThumbnail(KeyValuePair<string, ExpandedArticle> articleDetails, string archetypeWebPageUrl);
        string ArchetypeThumbnail(string thumbNailUrl, string archetypeWebPageUrl);
    }
}