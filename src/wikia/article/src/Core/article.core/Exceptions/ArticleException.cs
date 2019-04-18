using System;
using wikia.Models.Article.AlphabeticalList;

namespace article.core.Exceptions
{
    public class ArticleException
    {
        public UnexpandedArticle Article { get; set; }

        public Exception Exception { get; set; }
    }
}