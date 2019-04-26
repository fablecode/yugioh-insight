using System;
using System.Collections.Generic;
using article.core.Exceptions;

namespace article.core.Models
{
    public class ArticleBatchTaskResult
    {
        public Guid BatchId { get; set; }
        public string Category { get; set; }
        public int Processed { get; set; }
        public List<ArticleException> Failed { get; set; } = new List<ArticleException>();
        public Exception Exception { get; set; }
    }
}