using semanticsearch.core.Model;
using semanticsearch.core.Search;
using semanticsearch.domain.WebPage;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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

        public async Task Producer(string url, ITargetBlock<SemanticCard> targetBlock)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException(nameof(url));

            if (targetBlock == null)
                throw new ArgumentException(nameof(targetBlock));

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
                        await targetBlock.SendAsync(semanticCard);
                    }
                }

                if (_semanticSearchResults.HasNextPage)
                {
                    nextPageUrl = _semanticSearchResults.NextPageLink();
                }

            } while (_semanticSearchResults.HasNextPage);

            // Signals no more messages to produced or accepted.
            targetBlock.Complete();
        }
    }
}