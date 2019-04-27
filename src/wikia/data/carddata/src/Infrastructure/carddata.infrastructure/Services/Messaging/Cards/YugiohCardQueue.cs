using carddata.application.Configuration;
using carddata.core.Exceptions;
using carddata.core.Models;
using carddata.domain.Services.Messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Linq;
using System.Threading.Tasks;

namespace carddata.infrastructure.Services.Messaging.Cards
{
    public class YugiohCardQueue : IYugiohCardQueue
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;

        public YugiohCardQueue(IOptions<RabbitMqSettings> rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task<YugiohCardCompletion> Publish(ArticleProcessed articleProcessed)
        {
            var yugiohCardCompletion = new YugiohCardCompletion();
            yugiohCardCompletion.Card = articleProcessed.Card;
            yugiohCardCompletion.Article = articleProcessed.Article;

            try
            {
                var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(articleProcessed.Card));

                var factory = new ConnectionFactory() { HostName = _rabbitMqConfig.Value.Host };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var props = channel.CreateBasicProperties();
                    props.ContentType = _rabbitMqConfig.Value.ContentType;
                    props.DeliveryMode = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersData].PersistentMode;

                    props.Headers = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersData].Headers.ToDictionary(k => k.Key, k => (object)k.Value);

                    channel.BasicPublish
                    (
                        RabbitMqExchangeConstants.YugiohHeadersData,
                        "",
                        props,
                        messageBodyBytes
                    );
                }

                yugiohCardCompletion.IsSuccessful = true;
            }
            catch (System.Exception ex)
            {
                yugiohCardCompletion.Exception = new YugiohCardException { Card = articleProcessed.Card, Exception = ex };
            }

            return Task.FromResult(yugiohCardCompletion);
        }
    }
}