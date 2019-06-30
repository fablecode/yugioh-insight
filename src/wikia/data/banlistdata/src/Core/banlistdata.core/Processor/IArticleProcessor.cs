using System.Threading.Tasks;
using banlistdata.core.Models;

namespace banlistdata.core.Processor
{
    public interface IArticleProcessor
    {
        Task<ArticleConsumerResult> Process(Article article);
    }
}