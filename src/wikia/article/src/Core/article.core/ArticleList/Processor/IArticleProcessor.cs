using System.Threading.Tasks;
using article.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace article.core.ArticleList.Processor
{
    public interface IArticleProcessor
    {
        Task<ArticleTaskResult> Process(string category, UnexpandedArticle article);
    }
}