using System.Threading.Tasks;
using cardsectiondata.core.Models;

namespace cardsectiondata.core.Processor
{
    public interface IArticleItemProcessor
    {
        Task<ArticleTaskResult> ProcessItem(Article article);

        bool Handles(string category);
    }
}