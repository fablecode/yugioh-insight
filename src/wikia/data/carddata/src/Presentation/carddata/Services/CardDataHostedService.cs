﻿using carddata.application.Configuration;
using carddata.application.MessageConsumers.CardInformation;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace carddata.Services
{
    public sealed class CardDataHostedService : BackgroundService
    {
        private readonly ILogger<CardDataHostedService> _logger;
        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IMediator _mediator;

        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public CardDataHostedService
        (
            ILogger<CardDataHostedService> logger, 
            IOptions<RabbitMqSettings> rabbitMqOptions,
            IMediator mediator
        )
        {
            _logger = logger;
            _rabbitMqOptions = rabbitMqOptions;
            _mediator = mediator;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _factory = new ConnectionFactory
            {
                HostName = _rabbitMqOptions.Value.Host,
                UserName = _rabbitMqOptions.Value.Username,
                Password = _rabbitMqOptions.Value.Password,
                DispatchConsumersAsync = true
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.BasicQos(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardInformationConsumer { Message = message }, stoppingToken);


                    if (result.ArticleConsumerResult.IsSuccessfullyProcessed)
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                    }

                    await Task.Yield();
                }
                catch (Exception ex)
                {
                    _logger.LogError("RabbitMq Consumer: '{CardArticleQueue}' exception. Exception: {@Exception}", _rabbitMqOptions.Value.Queues.CardArticleQueue, ex);
                }
            };

            consumer.Shutdown += OnCardArticleConsumerShutdown;
            consumer.Registered += OnCardArticleConsumerRegistered;
            consumer.Unregistered += OnCardArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnCardArticleConsumerCancelled;

            _channel.BasicConsume(_rabbitMqOptions.Value.Queues.CardArticleQueue, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }


        #region private helper

        private Task OnCardArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation("RabbitMq Consumer '{CardArticleQueue}' Cancelled", _rabbitMqOptions.Value.Queues.CardArticleQueue);
            return Task.CompletedTask;
        }

        private Task OnCardArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation("RabbitMq Consumer '{CardArticleQueue}' Unregistered", _rabbitMqOptions.Value.Queues.CardArticleQueue);
            return Task.CompletedTask;
        }

        private Task OnCardArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation("RabbitMq Consumer '{CardArticleQueue}' Registered", _rabbitMqOptions.Value.Queues.CardArticleQueue);
            return Task.CompletedTask;
        }
        private Task OnCardArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("RabbitMq Consumer '{CardArticleQueue}' Shutdown", _rabbitMqOptions.Value.Queues.CardArticleQueue);
            return Task.CompletedTask;
        }

        #endregion
    }
}
