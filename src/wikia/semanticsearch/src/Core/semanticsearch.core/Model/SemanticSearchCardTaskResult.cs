using semanticsearch.core.Exceptions;
using System.Collections.Generic;

namespace semanticsearch.core.Model
{
    public class SemanticSearchCardTaskResult
    {
        public bool IsSuccessful { get; set; }

        public string Url { get; set; }

        public int Processed { get; set; }

        public List<SemanticCardPublishException> Failed { get; set; } = new();
    }
}