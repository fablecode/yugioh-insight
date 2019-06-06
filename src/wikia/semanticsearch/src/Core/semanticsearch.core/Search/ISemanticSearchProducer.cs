using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using semanticsearch.core.Model;

namespace semanticsearch.core.Search
{
    public interface ISemanticSearchProducer
    {
        Task Producer(string url, ITargetBlock<SemanticCard> targetBlock);
    }
}