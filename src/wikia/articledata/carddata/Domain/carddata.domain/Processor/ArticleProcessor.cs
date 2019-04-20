using System.Threading.Tasks;
using carddata.core.Models;
using carddata.core.Processor;

namespace carddata.domain.Processor
{
    public class ArticleProcessor : IArticleProcessor
    {
        public ArticleProcessor()
        {
            
        }
        public Task<ArticleConsumerResult> Process(Article article)
        {
            throw new System.NotImplementedException();
        }
    }
}