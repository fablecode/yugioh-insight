using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using banlistdata.application.Configuration;
using banlistdata.application.MessageConsumers.BanlistInformation;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace banlistdata.Services
{
    public class BanlistDataHostedService : BackgroundService
    {
        private const string BanlistArticleQueue = "banlist-article";

        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;

        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _banlistArticleChannel;

        private EventingBasicConsumer _banlistArticleConsumer;

        public BanlistDataHostedService
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

            _banlistArticleConsumer = CreateCardArticleConsumer(_connection);

            return Task.CompletedTask;
        }

        private EventingBasicConsumer CreateCardArticleConsumer(IConnection connection)
        {
            _banlistArticleChannel = connection.CreateModel();
            _banlistArticleChannel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_banlistArticleChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new BanlistInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _banlistArticleChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _banlistArticleChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + BanlistArticleQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnCardArticleConsumerShutdown;
            consumer.Registered += OnCardArticleConsumerRegistered;
            consumer.Unregistered += OnCardArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnCardArticleConsumerCancelled;

            _banlistArticleChannel.BasicConsume(queue: BanlistArticleQueue,
                autoAck: false,
                consumer: consumer);

            return consumer;
        }

        public override void Dispose()
        {
            _banlistArticleChannel.Close();
            _connection.Close();
            base.Dispose();
        }

        private static void OnCardArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{BanlistArticleQueue}' Cancelled");
        }

        private static void OnCardArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{BanlistArticleQueue}' Unregistered");}

        private static void OnCardArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{BanlistArticleQueue}' Registered");
        }
        private static void OnCardArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{BanlistArticleQueue}' Shutdown");
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
                    (_appSettingsOptions.Value.LogFolder + $@"/banlistdata.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        #endregion
    }
}
