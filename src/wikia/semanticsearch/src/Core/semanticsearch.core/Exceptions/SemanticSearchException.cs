using System;

namespace semanticsearch.core.Exceptions
{
    public class SemanticSearchException
    {
        public string Url { get; set; }

        public Exception Exception { get; set; }
    }
}