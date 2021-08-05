using semanticsearch.core.Model;
using semanticsearch.core.Search;
using semanticsearch.domain.Messaging.Exchanges;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace semanticsearch.domain.Search.Consumer
{
    public class SemanticSearchConsumer : ISemanticSearchConsumer
    {
        private readonly ILogger<SemanticSearchConsumer> _logger;
        private readonly IArticleHeaderExchange _articleHeaderExchange;

        public SemanticSearchConsumer(ILogger<SemanticSearchConsumer> logger, IArticleHeaderExchange articleHeaderExchange)
        {
            _logger = logger;
            _articleHeaderExchange = articleHeaderExchange;
        }
        public async Task<SemanticCardPublishResult> Process(SemanticCard semanticCard)
        {
            var response = new SemanticCardPublishResult
            {
                Card = semanticCard
            };

            _logger.LogInformation("Publishing semantic card '{CardName}' to queue", semanticCard.Title);

            await _articleHeaderExchange.Publish(semanticCard);

            response.IsSuccessful = true;

            return response;
        }
    }
}