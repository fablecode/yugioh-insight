using System.Threading.Tasks;
using semanticsearch.core.Model;
using semanticsearch.core.Search;

namespace semanticsearch.domain.Search.Consumer
{
    public class SemanticSearchConsumer : ISemanticSearchConsumer
    {
        public Task<SemanticCardPublishResult> Consumer(SemanticCard semanticCard)
        {
            throw new System.NotImplementedException();
        }
    }
}