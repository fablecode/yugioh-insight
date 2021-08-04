using semanticsearch.core.Model;
using semanticsearch.core.Search;
using semanticsearch.domain.WebPage;
using System;
using System.Collections.Generic;

namespace semanticsearch.domain.Search.Producer
{
    public class SemanticSearchProducer : ISemanticSearchProducer
    {
        private readonly ISemanticSearchResultsWebPage _semanticSearchResults;
        private readonly ISemanticCardSearchResultsWebPage _semanticCardSearchResults;

        public SemanticSearchProducer(ISemanticSearchResultsWebPage semanticSearchResults, ISemanticCardSearchResultsWebPage semanticCardSearchResults)
        {
            _semanticSearchResults = semanticSearchResults;
            _semanticCardSearchResults = semanticCardSearchResults;
        }

        public IEnumerable<SemanticCard> Producer(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException(nameof(url));

            var nextPageUrl = url;

            do
            {
                _semanticSearchResults.Load(nextPageUrl);

                foreach (var row in _semanticSearchResults.TableRows)
                {
                    var semanticCard = new SemanticCard
                    {
                        Title = _semanticCardSearchResults.Name(row),
                        CorrelationId = Guid.NewGuid(),
                        Url = _semanticCardSearchResults.Url(row, _semanticSearchResults.CurrentWebPageUri)
                    };

                    if (!string.IsNullOrWhiteSpace(semanticCard.Title) && !string.IsNullOrWhiteSpace(semanticCard.Url))
                    {
                        yield return semanticCard;
                    }
                }

                if (_semanticSearchResults.HasNextPage)
                {
                    nextPageUrl = _semanticSearchResults.NextPageLink();
                }

            } while (_semanticSearchResults.HasNextPage);
        }
    }
}