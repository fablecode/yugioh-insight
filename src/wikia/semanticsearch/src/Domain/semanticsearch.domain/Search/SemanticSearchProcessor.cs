using semanticsearch.core.Model;
using semanticsearch.core.Search;
using System.Threading.Tasks;

namespace semanticsearch.domain.Search
{
    public class SemanticSearchProcessor : ISemanticSearchProcessor
    {
        private readonly ISemanticSearchProducer _semanticSearchProducer;
        private readonly ISemanticSearchConsumer _semanticSearchConsumer;

        public SemanticSearchProcessor(ISemanticSearchProducer semanticSearchProducer, ISemanticSearchConsumer semanticSearchConsumer)
        {
            _semanticSearchProducer = semanticSearchProducer;
            _semanticSearchConsumer = semanticSearchConsumer;
        }

        public async Task<SemanticSearchCardTaskResult> ProcessUrl(string url)
        {
            var response = new SemanticSearchCardTaskResult { Url = url };

            foreach (var semanticCard in _semanticSearchProducer.Producer(url))
            {
                var result = await _semanticSearchConsumer.Process(semanticCard);

                if (result.IsSuccessful)
                    response.Processed += 1;
                else
                {
                    response.Failed.Add(result.Exception);
                }
            }

            response.IsSuccessful = true;

            return response;
        }
    }
}