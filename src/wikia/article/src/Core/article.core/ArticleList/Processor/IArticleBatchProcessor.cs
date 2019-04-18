using System.Threading.Tasks;
using article.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace article.core.ArticleList.Processor
{
    public interface IArticleBatchProcessor
    {
        Task<ArticleBatchTaskResult> Process(string category, UnexpandedArticle[] articles);
    }
}