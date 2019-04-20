using System.Threading.Tasks;
using articledata.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.core.ArticleList.Processor
{
    public interface IArticleProcessor
    {
        Task<ArticleTaskResult> Process(string category, UnexpandedArticle article);
    }
}