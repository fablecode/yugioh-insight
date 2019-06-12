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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace carddata.Services
{
    public class CardDataHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;

        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _cardArticleChannel;

        private EventingBasicConsumer _cardArticleConsumer;

        public CardDataHostedService
        (
            IServiceProvider services, 
            IOptions<RabbitMqSettings> rabbitMqOptions,
            IOptions<AppSettings> appSettingsOptions,
            IMediator mediator
        )
        {
            Services = services;
            _rabbitMqOptions = rabbitMqOptions;
            _appSettingsOptions = appSettingsOptions;
            _mediator = mediator;

            ConfigureSerilog();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await StartConsumer();
        }

        #region private helper 
        private Task StartConsumer()
        {
            _factory = new ConnectionFactory() { HostName = _rabbitMqOptions.Value.Host };
            _connection = _factory.CreateConnection();

            _cardArticleConsumer = CreateCardArticleConsumer(_connection);

            return Task.CompletedTask;
        }

        private EventingBasicConsumer CreateCardArticleConsumer(IConnection connection)
        {
            _cardArticleChannel = connection.CreateModel();
            _cardArticleChannel.BasicQos(0, 20, false);

            var consumer = new EventingBasicConsumer(_cardArticleChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardInformationConsumer { Message = message });


                    if (result.ArticleConsumerResult.IsSuccessfullyProcessed)
                    {
                        _cardArticleChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _cardArticleChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: card-article exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnCardArticleConsumerShutdown;
            consumer.Registered += OnCardArticleConsumerRegistered;
            consumer.Unregistered += OnCardArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnCardArticleConsumerCancelled;

            _cardArticleChannel.BasicConsume(queue: "card-article",
                autoAck: false,
                consumer: consumer);

            return consumer;
        }

        public override void Dispose()
        {
            _cardArticleChannel.Close();
            _connection.Close();
            base.Dispose();
        }

        private void OnCardArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information("Consumer 'card-article' Cancelled");
        }

        private void OnCardArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information("Consumer 'card-article' Unregistered");}

        private void OnCardArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information("Consumer 'card-article' Registered");
        }
        private void OnCardArticleConsumerShutdown(object sender, ShutdownEventArgs e)
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
