using System;
using System.Threading.Tasks;
using cardsectiondata.core;
using cardsectiondata.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace cardsectiondata.domain
{
    public class ArticleProcessor : IArticleProcessor
    {
        public Task<ArticleTaskResult> Process(string category, UnexpandedArticle article)
        {
            throw new NotImplementedException();
        }
    }
}
