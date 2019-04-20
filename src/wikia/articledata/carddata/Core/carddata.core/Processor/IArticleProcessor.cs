using System.Threading.Tasks;
using carddata.core.Models;

namespace carddata.core.Processor
{
    public interface IArticleProcessor
    {
        Task<ArticleConsumerResult> Process(Article article);
    }
}