using System.Collections.Generic;
using semanticsearch.core.Exceptions;
using semanticsearch.core.Search;

namespace semanticsearch.core.Model
{
    public class SemanticSearchCardTaskResult
    {
        public bool IsSuccessful { get; set; }

        public string Url { get; set; }

        public int Processed { get; set; }

        public List<SemanticCardPublishException> Failed { get; set; } = new List<SemanticCardPublishException>();
    }
}