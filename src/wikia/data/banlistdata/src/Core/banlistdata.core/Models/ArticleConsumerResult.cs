using banlistdata.core.Exceptions;

namespace banlistdata.core.Models
{
    public class ArticleConsumerResult
    {
        public bool IsSuccessfullyProcessed { get; set; }
        public string Article { get; set; }
        public ArticleException Failed { get; set; }
    }
}