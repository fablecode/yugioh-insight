using cardsectiondata.core.Models;
using System.Threading.Tasks;

namespace cardsectiondata.core.Processor
{
    public interface IArticleProcessor
    {
        Task<ArticleTaskResult> Process(string category, Article article);
    }
}
