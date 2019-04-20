using articledata.core.Exceptions;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.core.Models
{
    public class ArticleTaskResult
    {
        public bool IsSuccessfullyProcessed { get; set; }

        public UnexpandedArticle Article { get; set; }

        public ArticleException Failed { get; set; }

        public object Data { get; set; }
    }
}