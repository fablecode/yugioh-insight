using System;

namespace carddata.core.Models
{
    public class ArticleCompletion
    {
        public bool IsSuccessful { get; set; }
        public Article Message { get; set; }
        public Exception Exception { get; set; }
    }

    public class ArticleProcessed
    {
        public Article Article { get; set; }
        public YugiohCard Card { get; set; }
    }
}