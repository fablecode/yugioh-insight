using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using articledata.domain.mo;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.ArticleList.Processor
{
    public class ArticleProcessor : IArticleProcessor
    {
        private readonly IEnumerable<IArticleItemProcessor> _articleItemProcessors;

        public ArticleProcessor(IEnumerable<IArticleItemProcessor> articleItemProcessors)
        {
            _articleItemProcessors = articleItemProcessors;
        }

        public Task<ArticleTaskResult> Process(string category, UnexpandedArticle article)
        {
            var handler = _articleItemProcessors.Single(h => h.Handles(category));

            return handler.ProcessItem(article);
        }
    }
}