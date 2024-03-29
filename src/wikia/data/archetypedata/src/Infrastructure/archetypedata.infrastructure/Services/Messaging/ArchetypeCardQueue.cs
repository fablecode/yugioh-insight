﻿using System;
using System.Linq;
using System.Threading.Tasks;
using archetypedata.application.Configuration;
using archetypedata.core.Models;
using archetypedata.domain.Services.Messaging;
using archetypedata.infrastructure.Services.Messaging.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace archetypedata.infrastructure.Services.Messaging
{
    public class ArchetypeCardQueue : IQueue<ArchetypeCard>
    {
        private readonly ILogger<ArchetypeQueue> _logger;
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;

        public ArchetypeCardQueue(ILogger<ArchetypeQueue> logger, IOptions<RabbitMqSettings> rabbitMqConfig)
        {
            _logger = logger;
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task Publish(ArchetypeCard message)
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
                        .Queues[RabbitMqQueueConstants.ArchetypeCardData]
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
                _logger.LogError("Unable to send Archetype Card message to queue '{ArchetypeCardMessage}'. Exception: {Exception}", message, ex);
                throw;
            }

            return Task.CompletedTask;
        }
    }
}