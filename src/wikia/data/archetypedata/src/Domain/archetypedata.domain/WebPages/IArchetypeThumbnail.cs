using System.Collections.Generic;
using System.Threading.Tasks;
using wikia.Models.Article.Details;

namespace archetypedata.domain.WebPages
{
    public interface IArchetypeThumbnail
    {
        Task<string> FromArticleId(int articleId);
        string FromArticleDetails(KeyValuePair<string, ExpandedArticle> articleDetails);
        string FromWebPage(string url);
    }
}