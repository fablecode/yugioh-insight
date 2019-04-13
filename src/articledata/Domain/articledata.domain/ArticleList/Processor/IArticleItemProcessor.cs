using System.Threading.Tasks;
using articledata.domain.mo;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.ArticleList.Processor
{
    public interface IArticleItemProcessor
    {
        Task<ArticleTaskResult> ProcessItem(UnexpandedArticle item);

        bool Handles(string category);
    }
}