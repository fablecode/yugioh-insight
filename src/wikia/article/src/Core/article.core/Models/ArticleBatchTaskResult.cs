using article.core.Exceptions;
using System.Collections.Generic;

namespace article.core.Models
{
    public class ArticleBatchTaskResult
    {
        public string Category { get; set; }
        public int Processed { get; set; }
        public List<ArticleException> Failed { get; set; } = new List<ArticleException>();
    }
}