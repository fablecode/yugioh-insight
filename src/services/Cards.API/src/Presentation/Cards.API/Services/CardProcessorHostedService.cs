using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cards.Application.Configuration;
using Cards.Application.MessageConsumers.CardData;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Cards.API.Services
{
    public sealed class CardProcessorHostedService : BackgroundService
    {
        private const string CardDataQueue = "card-data";

        private readonly ILogger<CardProcessorHostedService> _logger;
        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IMediator _mediator;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public CardProcessorHostedService
        (
            ILogger<CardProcessorHostedService> logger,
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
                Port = _rabbitMqOptions.Value.Port,
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

                    var result = await _mediator.Send(new CardInformationConsumer(message), stoppingToken);


                    if (result.IsSuccessful)
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                    }

                    await Task.Yield();
                }
                catch (RabbitMQClientException ex)
                {
                    _logger.LogError("RabbitMq Consumer: '{CardArticleQueue}' exception. Exception: {@Exception}", _rabbitMqOptions.Value.Queues[CardDataQueue], ex);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Unexpected exception occurred for RabbitMq Consumer: '{CardArticleQueue}'. Exception: {@Exception}", _rabbitMqOptions.Value.Queues[CardDataQueue], ex);
                }
            };

            _channel.BasicConsume(_rabbitMqOptions.Value.Queues[CardDataQueue].Name, _rabbitMqOptions.Value.Queues[CardDataQueue].AutoAck, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}