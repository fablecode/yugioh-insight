using System;
using System.Linq;
using System.Threading.Tasks;
using banlistdata.application.Configuration;
using banlistdata.core.Models;
using banlistdata.domain.Services.Messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace banlistdata.infrastructure.Services.Messaging
{
    public class BanlistDataQueue : IBanlistDataQueue
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;

        public BanlistDataQueue(IOptions<RabbitMqSettings> rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task<YugiohBanlistCompletion> Publish(ArticleProcessed articleProcessed)
        {
            var yugiohBanlistCompletion = new YugiohBanlistCompletion();

            yugiohBanlistCompletion.Article = articleProcessed.Article;
            yugiohBanlistCompletion.Banlist = articleProcessed.Banlist;

            try
            {
                var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(articleProcessed.Banlist));

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

                yugiohBanlistCompletion.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return Task.FromResult(yugiohBanlistCompletion);
        }
    }
}