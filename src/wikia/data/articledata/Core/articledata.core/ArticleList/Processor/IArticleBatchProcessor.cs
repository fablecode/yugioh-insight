using System.Threading.Tasks;
using articledata.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.core.ArticleList.Processor
{
    public interface IArticleBatchProcessor
    {
        Task<ArticleBatchTaskResult> Process(string category, UnexpandedArticle[] articles);
    }
}