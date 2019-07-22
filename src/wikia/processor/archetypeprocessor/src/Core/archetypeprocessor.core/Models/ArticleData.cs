using System;

namespace archetypeprocessor.core.Models
{
    public class ArticleData
    {
        public long Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}