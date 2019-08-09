using System;
using System.Linq;
using System.Threading.Tasks;
using cardsectiondata.application.Configuration;
using cardsectiondata.core.Constants;
using cardsectiondata.core.Models;
using cardsectiondata.domain.Services.Messaging;
using cardsectiondata.infrastructure.Services.Messaging.Constants;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace cardsectiondata.infrastructure.Services.Messaging
{
    public class CardRulingQueue : IQueue
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;

        public CardRulingQueue(IOptions<RabbitMqSettings> rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task Publish(CardSectionMessage message)
        {
            try
            {
                var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var factory = new ConnectionFactory() { HostName = _rabbitMqConfig.Value.Host };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var props = channel.CreateBasicProperties();
                    props.ContentType = _rabbitMqConfig.Value.ContentType;
                    props.DeliveryMode = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersData].PersistentMode;

                    props.Headers = _rabbitMqConfig.Value
                        .Exchanges[RabbitMqExchangeConstants.YugiohHeadersData]
                        .Queues[RabbitMqQueueConstants.CardRulingsData]
                        .Headers.ToDictionary(k => k.Key, k => (object)k.Value);

                    channel.BasicPublish
                    (
                        RabbitMqExchangeConstants.YugiohHeadersData,
                        "",
                        props,
                        messageBodyBytes
                    );
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return Task.CompletedTask;
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardRulings;
        }
    }
}