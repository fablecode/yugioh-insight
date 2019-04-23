using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardprocessor.application.Configuration;
using cardprocessor.core.Models;
using cardprocessor.domain.Queues.Cards;
using cardprocessor.domain.Services.Messaging.Cards;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace cardprocessor.infrastructure.Services.Messaging.Cards
{
    public class CardImageQueue : ICardImageQueue
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;

        public CardImageQueue(IOptions<RabbitMqSettings> rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task<CardImageCompletion> Publish(DownloadImage downloadImage)
        {
            var cardImageCompletion = new CardImageCompletion();

            try
            {
                var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(downloadImage));

                var factory = new ConnectionFactory() { HostName = _rabbitMqConfig.Value.Host };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var props = channel.CreateBasicProperties();
                    props.ContentType = _rabbitMqConfig.Value.ContentType;
                    props.DeliveryMode = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersImage].PersistentMode;

                    props.Headers = _rabbitMqConfig.Value.Exchanges[RabbitMqExchangeConstants.YugiohHeadersImage].Headers.ToDictionary(k => k.Key, k => (object)k.Value);

                    channel.BasicPublish
                    (
                        RabbitMqExchangeConstants.YugiohHeadersImage,
                        "",
                        props,
                        messageBodyBytes
                    );
                }

                cardImageCompletion.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                cardImageCompletion.Errors = new List<string>{ $"Unable to send image: {downloadImage.RemoteImageUrl} to download queue"};
                cardImageCompletion.Exception = ex;
            }

            return Task.FromResult(cardImageCompletion);
        }
    }
}