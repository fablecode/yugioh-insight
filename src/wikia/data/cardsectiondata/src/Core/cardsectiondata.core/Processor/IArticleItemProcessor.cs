using System.Threading.Tasks;
using cardsectiondata.core.Models;

namespace cardsectiondata.core.Processor
{
    public interface IArticleItemProcessor
    {
        Task<ArticleTaskResult> ProcessItem(Article item);

        bool Handles(string category);
    }
}