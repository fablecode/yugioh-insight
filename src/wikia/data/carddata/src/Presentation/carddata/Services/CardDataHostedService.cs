using carddata.application.Configuration;
using carddata.application.MessageConsumers.CardInformation;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace carddata.Services
{
    public class CardDataHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;
        private readonly IHost _host;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _cardArticleConsumer;

        public CardDataHostedService
        (
            IServiceProvider services, 
            IOptions<RabbitMqSettings> rabbitMqOptions,
            IOptions<AppSettings> appSettingsOptions,
            IMediator mediator,
            IHost host
        )
        {
            Services = services;
            _rabbitMqOptions = rabbitMqOptions;
            _appSettingsOptions = appSettingsOptions;
            _mediator = mediator;
            _host = host;

            ConfigureSerilog();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await StartConsumer();
        }

        #region private helper 
        private async Task StartConsumer()
        {
            _factory = new ConnectionFactory() { HostName = _rabbitMqOptions.Value.Host };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.BasicQos(1, 20, false);

            _cardArticleConsumer = new EventingBasicConsumer(_channel);

            _cardArticleConsumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardInformationConsumer { Message = message });

                    if (result.ArticleConsumerResult.IsSuccessfullyProcessed)
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
                    Log.Logger.Error("RabbitMq Consumer: card-article exception. Exception: {@Exception}", ex);
                }
            };

            _cardArticleConsumer.Shutdown += OnConsumerShutdown;
            _cardArticleConsumer.Registered += OnConsumerRegistered;
            _cardArticleConsumer.Unregistered += OnConsumerUnregistered;
            _cardArticleConsumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(queue: "card-article",
                autoAck: false,
                consumer: _cardArticleConsumer);

            await _host.WaitForShutdownAsync();
        }

        public override void Dispose()
        {
            _connection.Close();
            _channel.Close();
            base.Dispose();
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information("Consumer 'card-article' Cancelled");
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information("Consumer 'card-article' Unregistered");}

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information("Consumer 'card-article' Registered");
        }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information("Consumer 'card-article' Shutdown");
        }

        private void ConfigureSerilog()
        {
            // Create the logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(renderMessage: true),
                    (_appSettingsOptions.Value.LogFolder + $@"/carddata.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        #endregion
    }
}
