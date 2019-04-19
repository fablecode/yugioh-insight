using article.application.Configuration;
using article.core.Models;
using article.domain.Services.Messaging.Cards;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Linq;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.Services.Messaging.Cards
{
    public class CardArticleQueue : ICardArticleQueue
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;

        public CardArticleQueue(IOptions<RabbitMqSettings> rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task Publish(UnexpandedArticle article)
        {
            var messageToBeSent = new Article
            {
                Id = article.Id,
                Title = article.Title,
                Url = article.Url
            };

            var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageToBeSent));

            var factory = new ConnectionFactory() { HostName = _rabbitMqConfig.Value.Host };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var props = channel.CreateBasicProperties();
                props.ContentType = _rabbitMqConfig.Value.ContentType;
                props.DeliveryMode = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersArticle].PersistentMode;
                props.Headers = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersArticle].Headers.ToDictionary(k => k.Key, k => (object) k.Value);

                channel.BasicPublish
                (
                    RabbitMqExchangeConstants.YugiohHeadersArticle,
                    string.Empty, 
                    props,
                    messageBodyBytes
                );
            }

            return Task.CompletedTask;
        }
    }
}