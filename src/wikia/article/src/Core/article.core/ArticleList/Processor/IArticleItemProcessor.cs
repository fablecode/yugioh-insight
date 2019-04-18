using System.Threading.Tasks;
using article.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace article.core.ArticleList.Processor
{
    public interface IArticleItemProcessor
    {
        Task<ArticleTaskResult> ProcessItem(UnexpandedArticle item);

        bool Handles(string category);
    }
}