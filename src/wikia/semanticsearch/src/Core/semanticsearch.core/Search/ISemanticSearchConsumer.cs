using System.Threading.Tasks;
using semanticsearch.core.Model;

namespace semanticsearch.core.Search
{
    public interface ISemanticSearchConsumer
    {
        Task<SemanticCardPublishResult> Consumer(SemanticCard semanticCard);
    }
}