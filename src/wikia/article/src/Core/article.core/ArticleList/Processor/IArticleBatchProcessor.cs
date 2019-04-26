using article.core.Models;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.core.ArticleList.Processor
{
    public interface IArticleBatchProcessor
    {
        Task<ArticleBatchTaskResult> Process(string category, UnexpandedArticle[] articles);
    }
}