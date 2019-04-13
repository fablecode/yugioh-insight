using articledata.domain.ArticleList.DataSource;
using articledata.domain.Exceptions;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.mo
{
    public class ArticleTaskResult
    {
        public bool IsSuccessfullyProcessed { get; set; }

        public UnexpandedArticle Article { get; set; }

        public ArticleException Failed { get; set; }

        public object Data { get; set; }
    }
}