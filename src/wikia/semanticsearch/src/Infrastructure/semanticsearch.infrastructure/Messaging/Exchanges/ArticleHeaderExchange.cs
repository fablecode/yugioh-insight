using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using semanticsearch.application.Configuration;
using semanticsearch.core.Model;
using System.Linq;
using System.Threading.Tasks;
using semanticsearch.domain.Messaging.Exchanges;

namespace semanticsearch.infrastructure.Messaging.Exchanges
{
    public class ArticleHeaderExchange : IArticleHeaderExchange
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;

        public ArticleHeaderExchange(IOptions<RabbitMqSettings> rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task Publish(SemanticCard semanticCard)
        {
            var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(semanticCard));

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqConfig.Value.Host, 
                Port = _rabbitMqConfig.Value.Port,
                UserName = _rabbitMqConfig.Value.Username,
                Password = _rabbitMqConfig.Value.Password
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var props = channel.CreateBasicProperties();
                props.ContentType = _rabbitMqConfig.Value.ContentType;
                props.DeliveryMode = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersArticle].PersistentMode;
                props.Headers = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersArticle].Headers.ToDictionary(k => k.Key, k => (object)k.Value);

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