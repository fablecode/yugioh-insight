using System.Threading.Tasks;
using semanticsearch.core.Model;

namespace semanticsearch.core.Search
{
    public interface ISemanticSearchPublish
    {
        Task<SemanticSearchBatchTaskResult> Publish(string category, SemanticCard semanticCard);
    }
}