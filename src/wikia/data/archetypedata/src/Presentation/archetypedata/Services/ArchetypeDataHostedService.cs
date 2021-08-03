﻿using archetypedata.application.Configuration;
using archetypedata.application.MessageConsumers.ArchetypeInformation;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace archetypedata.Services
{
    public sealed class ArchetypeDataHostedService : BackgroundService
    {
        private const string ArchetypeArticleQueue = "archetype-article";

        public IServiceProvider Services { get; }

        private readonly ILogger<ArchetypeDataHostedService> _logger;
        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;

        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public ArchetypeDataHostedService
        (
            ILogger<ArchetypeDataHostedService> logger,
            IServiceProvider services, 
            IOptions<RabbitMqSettings> rabbitMqOptions,
            IOptions<AppSettings> appSettingsOptions,
            IMediator mediator
        )
        {
            Services = services;
            _logger = logger;
            _rabbitMqOptions = rabbitMqOptions;
            _appSettingsOptions = appSettingsOptions;
            _mediator = mediator;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _factory = new ConnectionFactory()
            {
                HostName = _rabbitMqOptions.Value.Host, 
                UserName = _rabbitMqOptions.Value.Username, 
                Password = _rabbitMqOptions.Value.Password
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new ArchetypeInformationConsumer { Message = message }, stoppingToken);


                    if (result.IsSuccessful)
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("RabbitMq Consumer: " + ArchetypeArticleQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnArchetypeArticleConsumerShutdown;
            consumer.Registered += OnArchetypeArticleConsumerRegistered;
            consumer.Unregistered += OnArchetypeArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnArchetypeArticleConsumerCancelled;

            _channel.BasicConsume(ArchetypeArticleQueue, false, consumer);

            return Task.CompletedTask;
        }

        #region private helper 

        private void OnArchetypeArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"Consumer '{ArchetypeArticleQueue}' Cancelled");
        }

        private void OnArchetypeArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"Consumer '{ArchetypeArticleQueue}' Unregistered");}

        private void OnArchetypeArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"Consumer '{ArchetypeArticleQueue}' Registered");
        }
        private void OnArchetypeArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"Consumer '{ArchetypeArticleQueue}' Shutdown");
        }

        #endregion

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
