using System.Threading.Tasks;
using wikia.Models.Article.Details;
using wikia.Models.Article.PageList;
using wikia.Models.Article.Simple;

namespace yugioh_insight.Domain.Client
{
    public interface IYugiohClient
    {
        Task<ExpandedListArticleResultSet> ArticlePageList(string category);
        Task<ContentResult> ArticleSimple(int article);
        Task<ExpandedArticleResultSet> ArticleDetails(params int[] articleIds);
    }
}