using semanticsearch.core.Model;
using System.Collections.Generic;

namespace semanticsearch.core.Search
{
    public interface ISemanticSearchProducer
    {
        IEnumerable<SemanticCard> Producer(string url);
    }
}