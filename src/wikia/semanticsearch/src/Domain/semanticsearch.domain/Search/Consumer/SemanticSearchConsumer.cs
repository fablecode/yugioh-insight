using semanticsearch.core.Model;
using semanticsearch.core.Search;
using semanticsearch.domain.Messaging.Exchanges;
using System.Threading.Tasks;

namespace semanticsearch.domain.Search.Consumer
{
    public class SemanticSearchConsumer : ISemanticSearchConsumer
    {
        private readonly IArticleHeaderExchange _articleHeaderExchange;

        public SemanticSearchConsumer(IArticleHeaderExchange articleHeaderExchange)
        {
            _articleHeaderExchange = articleHeaderExchange;
        }
        public async Task<SemanticCardPublishResult> Process(SemanticCard semanticCard)
        {
            var response = new SemanticCardPublishResult
            {
                Card = semanticCard
            };

            await _articleHeaderExchange.Publish(semanticCard);

            response.IsSuccessful = true;

            return response;
        }
    }
}