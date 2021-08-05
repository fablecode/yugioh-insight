using semanticsearch.core.Model;
using semanticsearch.core.Search;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace semanticsearch.domain.Search
{
    public class SemanticSearchProcessor : ISemanticSearchProcessor
    {
        private readonly ILogger<SemanticSearchProcessor> _logger;
        private readonly ISemanticSearchProducer _semanticSearchProducer;
        private readonly ISemanticSearchConsumer _semanticSearchConsumer;

        public SemanticSearchProcessor
        (
            ILogger<SemanticSearchProcessor> logger,
            ISemanticSearchProducer semanticSearchProducer, 
            ISemanticSearchConsumer semanticSearchConsumer
        )
        {
            _logger = logger;
            _semanticSearchProducer = semanticSearchProducer;
            _semanticSearchConsumer = semanticSearchConsumer;
        }

        public async Task<SemanticSearchCardTaskResult> ProcessUrl(string url)
        {
            var response = new SemanticSearchCardTaskResult { Url = url };

            foreach (var semanticCard in _semanticSearchProducer.Producer(url))
            {
                _logger.LogInformation("Processing semantic card '{CardName}'", semanticCard.Title);
                var result = await _semanticSearchConsumer.Process(semanticCard);

                if (result.IsSuccessful)
                    response.Processed += 1;
                else
                {
                    response.Failed.Add(result.Exception);
                }
                _logger.LogInformation("Finished processing semantic card '{CardName}'", semanticCard.Title);
            }

            response.IsSuccessful = true;

            return response;
        }
    }
}