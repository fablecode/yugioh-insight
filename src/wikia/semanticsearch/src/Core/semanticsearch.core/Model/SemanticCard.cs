using System;

namespace semanticsearch.core.Model
{
    public class SemanticCard
    {
        public Guid CorrelationId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}