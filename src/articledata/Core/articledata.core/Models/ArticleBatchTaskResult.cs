using System.Collections.Generic;
using articledata.core.Exceptions;

namespace articledata.core.Models
{
    public class ArticleBatchTaskResult
    {
        public string Category { get; set; }
        public int Processed { get; set; }
        public List<ArticleException> Failed { get; set; } = new List<ArticleException>();
    }
}