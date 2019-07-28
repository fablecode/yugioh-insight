using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardsectiondata.domain.ArticleList.Processor
{
    public class ArticleProcessor : IArticleProcessor
    {
        private readonly IEnumerable<IArticleItemProcessor> _articleItemProcessors;

        public ArticleProcessor(IEnumerable<IArticleItemProcessor> articleItemProcessors)
        {
            _articleItemProcessors = articleItemProcessors;
        }
        public Task<ArticleTaskResult> Process(string category, Article article)
        {
            var handler = _articleItemProcessors.Single(h => h.Handles(category));

            return handler.ProcessItem(article);
        }
    }
}
