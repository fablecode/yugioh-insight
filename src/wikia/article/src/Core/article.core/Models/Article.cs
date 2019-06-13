using System;

namespace article.core.Models
{
    public class Article
    {
        public int Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}