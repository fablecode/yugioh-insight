using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.ArticleList.DataSource
{
    public interface IArticleCategoryDataSource
    {
        Task Producer(string category, int pageSize, ITargetBlock<UnexpandedArticle[]> targetBlock);
    }
}