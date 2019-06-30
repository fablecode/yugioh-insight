using System.Threading.Tasks;
using banlistdata.core.Models;

namespace banlistdata.core.Processor
{
    public interface IArticleDataFlow
    {
        Task<ArticleCompletion> ProcessDataFlow(Article article);
    }
}