using System.Threading.Tasks;
using semanticsearch.core.Model;

namespace semanticsearch.core.Search
{
    public interface ISemanticSearchProcessor
    {
        Task<SemanticSearchCardTaskResult> ProcessUrl(string url);
    }
}