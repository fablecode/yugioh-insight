using wikia.Models.Article.AlphabeticalList;

namespace cardsectiondata.core.Models
{
    public class ArticleTaskResult
    {
        public bool IsSuccessfullyProcessed { get; set; }

        public UnexpandedArticle Article { get; set; }

        public ArticleException Failed { get; set; }
    }
}