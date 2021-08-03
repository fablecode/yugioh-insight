using archetypedata.application.Configuration;
using archetypedata.application.MessageConsumers.ArchetypeCardInformation;
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

namespace archetypedata.Services
{
    public sealed class ArchetypeCardDataHostedService : BackgroundService
    {
        private const string ArchetypeCardArticleQueue = "archetype-cards-article";

        private readonly ILogger<ArchetypeCardDataHostedService> _logger;
        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public ArchetypeCardDataHostedService
        (
            ILogger<ArchetypeCardDataHostedService> logger,
            IOptions<RabbitMqSettings> rabbitMqOptions,
            IOptions<AppSettings> appSettingsOptions,
            IMediator mediator
        )
        {
            _logger = logger;
            _rabbitMqOptions = rabbitMqOptions;
            _appSettingsOptions = appSettingsOptions;
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

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new ArchetypeCardInformationConsumer { Message = message }, stoppingToken);


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
                    _logger.LogError("RabbitMq Consumer: " + ArchetypeCardArticleQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnArchetypeCardArticleConsumerShutdown;
            consumer.Registered += OnArchetypeCardArticleConsumerRegistered;
            consumer.Unregistered += OnArchetypeCardArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnArchetypeCardArticleConsumerCancelled;

            _channel.BasicConsume(ArchetypeCardArticleQueue, false, consumer);

            return Task.CompletedTask;
        }

        #region private helper

        private Task OnArchetypeCardArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation("Consumer '{ArchetypeCardArticleQueue}' Cancelled", ArchetypeCardArticleQueue);
            return Task.CompletedTask;
        }

        private Task OnArchetypeCardArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation("Consumer '{ArchetypeCardArticleQueue}' Unregistered", ArchetypeCardArticleQueue);
            return Task.CompletedTask;
        }

        private Task OnArchetypeCardArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation("Consumer '{ArchetypeCardArticleQueue}' Registered", ArchetypeCardArticleQueue);
            return Task.CompletedTask;
        }
        private Task OnArchetypeCardArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("Consumer '{ArchetypeCardArticleQueue}' Shutdown", ArchetypeCardArticleQueue);
            return Task.CompletedTask;
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