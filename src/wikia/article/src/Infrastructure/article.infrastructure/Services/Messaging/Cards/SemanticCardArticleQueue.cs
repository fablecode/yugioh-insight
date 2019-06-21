using System;
using System.Linq;
using System.Threading.Tasks;
using article.application.Configuration;
using article.core.Models;
using article.domain.Services.Messaging.Cards;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.Services.Messaging.Cards
{
    public class SemanticCardArticleQueue : ISemanticCardArticleQueue
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;

        public SemanticCardArticleQueue(IOptions<RabbitMqSettings> rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task Publish(UnexpandedArticle article)
        {
            var messageToBeSent = new Article
            {
                Id = article.Id,
                CorrelationId = Guid.NewGuid()
            };

            var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageToBeSent));

            var factory = new ConnectionFactory() { HostName = _rabbitMqConfig.Value.Host };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var props = channel.CreateBasicProperties();
                props.ContentType = _rabbitMqConfig.Value.ContentType;
                props.DeliveryMode = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersArticleExchange].PersistentMode;
                props.Headers = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersArticleExchange].Headers.ToDictionary(k => k.Key, k => (object)k.Value);

                channel.BasicPublish
                (
                    RabbitMqExchangeConstants.YugiohHeadersArticleExchange,
                    string.Empty,
                    props,
                    messageBodyBytes
                );
            }

            return Task.CompletedTask;
        }
    }
}