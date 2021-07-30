using System;
using System.Linq;
using System.Threading.Tasks;
using article.application.Configuration;
using article.core.Models;
using article.domain.Services.Messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.Services.Messaging
{
    public class ArticleQueue : IQueue<Article>
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;
        private readonly IOptions<AppSettings> _appSettings;

        public ArticleQueue(IOptions<RabbitMqSettings> rabbitMqConfig, IOptions<AppSettings> appSettings)
        {
            _rabbitMqConfig = rabbitMqConfig;
            _appSettings = appSettings;
        }

        public Task Publish(UnexpandedArticle message)
        {
            var messageToBeSent = new Article
            {
                Id = message.Id,
                CorrelationId = Guid.NewGuid(),
                Title = message.Title,
                Url = new Uri(new Uri(_appSettings.Value.WikiaDomainUrl), message.Url).AbsoluteUri
            };

            return Publish(messageToBeSent);
        }

        public Task Publish(Article message)
        {
            var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var factory = new ConnectionFactory() { HostName = _rabbitMqConfig.Value.Host, Port = _rabbitMqConfig.Value.Port };
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