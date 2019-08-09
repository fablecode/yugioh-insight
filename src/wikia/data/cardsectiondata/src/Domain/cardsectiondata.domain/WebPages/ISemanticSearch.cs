using System.Collections.Generic;
using cardsectiondata.core.Models;

namespace cardsectiondata.domain.WebPages
{
    public interface ISemanticSearch
    {
        List<SemanticCard> CardsByUrl(string url);
    }
}