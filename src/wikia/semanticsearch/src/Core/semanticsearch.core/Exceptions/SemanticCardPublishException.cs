using System;

namespace semanticsearch.core.Exceptions
{
    public class SemanticCardPublishException
    {
        public string Url { get; set; }

        public Exception Exception { get; set; }
    }
}