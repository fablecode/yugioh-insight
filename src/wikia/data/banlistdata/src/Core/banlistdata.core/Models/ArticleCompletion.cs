using System;

namespace banlistdata.core.Models
{
    public class ArticleCompletion
    {
        public bool IsSuccessful { get; set; }
        public Article Message { get; set; }
        public Exception Exception { get; set; }
    }
}