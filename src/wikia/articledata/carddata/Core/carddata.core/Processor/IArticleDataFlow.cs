using System.Threading.Tasks;
using carddata.core.Models;

namespace carddata.core.Processor
{
    public interface IArticleDataFlow
    {
        Task<ArticleCompletion> ProcessDataFlow(Article article);
    }
}