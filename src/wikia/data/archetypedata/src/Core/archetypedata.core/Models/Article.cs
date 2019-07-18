using System;

namespace archetypedata.core.Models
{
    public class Article
    {
        public long Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}