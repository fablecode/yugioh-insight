using System.Collections.Generic;
using semanticsearch.core.Exceptions;
using semanticsearch.core.Search;

namespace semanticsearch.core.Model
{
    public class SemanticSearchBatchTaskResult
    {
        public bool IsSuccessful { get; set; }

        public string Url { get; set; }

        public int Processed { get; set; }

        public List<SemanticSearchException> Failed { get; set; } = new List<SemanticSearchException>();
    }
}