using System;
using System.Threading.Tasks;
using cardsectiondata.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace cardsectiondata.core
{
    public interface IArticleProcessor
    {
        Task<ArticleTaskResult> Process(string category, UnexpandedArticle article);
    }
}
