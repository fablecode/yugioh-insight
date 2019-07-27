using System;
using wikia.Models.Article.AlphabeticalList;

namespace cardsectiondata.core.Models
{
    public class ArticleException
    {
        public UnexpandedArticle Article { get; set; }

        public Exception Exception { get; set; }
    }
}