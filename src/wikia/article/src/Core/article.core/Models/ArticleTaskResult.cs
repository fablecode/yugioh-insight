using article.core.Exceptions;
using wikia.Models.Article.AlphabeticalList;

namespace article.core.Models
{
    public class ArticleTaskResult
    {
        public bool IsSuccessfullyProcessed { get; set; }

        public UnexpandedArticle Article { get; set; }

        public ArticleException Failed { get; set; }
    }
}