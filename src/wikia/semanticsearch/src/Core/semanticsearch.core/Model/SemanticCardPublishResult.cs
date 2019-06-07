using semanticsearch.core.Exceptions;

namespace semanticsearch.core.Model
{
    public class SemanticCardPublishResult
    {
        public bool IsSuccessful { get; set; }

        public SemanticCard Card { get; set; }

        public SemanticCardPublishException Exception { get; set; }
    }
}