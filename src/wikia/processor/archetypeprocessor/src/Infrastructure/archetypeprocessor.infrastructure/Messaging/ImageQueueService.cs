using System;
using System.Linq;
using archetypeprocessor.core.Models;
using archetypeprocessor.domain.Messaging;
using System.Threading.Tasks;
using archetypeprocessor.application.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace archetypeprocessor.infrastructure.Messaging
{
    public class ImageQueueService : IImageQueueService
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;

        public ImageQueueService(IOptions<RabbitMqSettings> rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task Publish(DownloadImage downloadImage)
        {
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

                    props.Headers = _rabbitMqConfig.Value
                        .Exchanges[RabbitMqExchangeConstants.YugiohHeadersImage]
                        .Queues[RabbitMqQueueConstants.ArchetypeImage]
                        .Headers.ToDictionary(k => k.Key, k => (object)k.Value);

                    channel.BasicPublish
                    (
                        RabbitMqExchangeConstants.YugiohHeadersImage,
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
    }
}