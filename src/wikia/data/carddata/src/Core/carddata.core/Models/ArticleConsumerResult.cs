using carddata.core.Exceptions;

namespace carddata.core.Models
{
    public class ArticleConsumerResult
    {
        public bool IsSuccessfullyProcessed { get; set; }
        public Article Article { get; set; }
        public ArticleException Failed { get; set; }
    }
}