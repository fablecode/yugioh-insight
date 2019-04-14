using System.Threading.Tasks;
using articledata.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.core.ArticleList.Processor
{
    public interface IArticleItemProcessor
    {
        Task<ArticleTaskResult> ProcessItem(UnexpandedArticle item);

        bool Handles(string category);
    }
}