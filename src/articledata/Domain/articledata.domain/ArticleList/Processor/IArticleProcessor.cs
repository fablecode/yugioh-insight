using System.Threading.Tasks;
using articledata.domain.mo;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.ArticleList.Processor
{
    public interface IArticleProcessor
    {
        Task<ArticleTaskResult> Process(string category, UnexpandedArticle article);
    }
}