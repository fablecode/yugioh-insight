using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using article.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.ArticleList.Processor
{
    public sealed class ArticleProcessor : IArticleProcessor
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