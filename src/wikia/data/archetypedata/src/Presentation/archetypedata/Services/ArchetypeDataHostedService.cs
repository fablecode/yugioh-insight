using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using archetypedata.application.Configuration;
using archetypedata.application.MessageConsumers.ArchetypeInformation;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace archetypedata.Services
{
    public class ArchetypeDataHostedService : BackgroundService
    {
        private const string ArchetypeArticleQueue = "archetype-article";

        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;

        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _archetypeArticleChannel;

        private EventingBasicConsumer _archetypeArticleConsumer;

        public ArchetypeDataHostedService
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

            _archetypeArticleConsumer = CreateArchetypeArticleConsumer(_connection);

            return Task.CompletedTask;
        }

        private EventingBasicConsumer CreateArchetypeArticleConsumer(IConnection connection)
        {
            _archetypeArticleChannel = connection.CreateModel();
            _archetypeArticleChannel.BasicQos(0, 20, false);

            var consumer = new EventingBasicConsumer(_archetypeArticleChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new ArchetypeInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _archetypeArticleChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _archetypeArticleChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + ArchetypeArticleQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnArchetypeArticleConsumerShutdown;
            consumer.Registered += OnArchetypeArticleConsumerRegistered;
            consumer.Unregistered += OnArchetypeArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnArchetypeArticleConsumerCancelled;

            _archetypeArticleChannel.BasicConsume
            (
                queue: ArchetypeArticleQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
        }

        public override void Dispose()
        {
            _archetypeArticleChannel.Close();
            _connection.Close();
            base.Dispose();
        }

        private static void OnArchetypeArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeArticleQueue}' Cancelled");
        }

        private static void OnArchetypeArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeArticleQueue}' Unregistered");}

        private static void OnArchetypeArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeArticleQueue}' Registered");
        }
        private static void OnArchetypeArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeArticleQueue}' Shutdown");
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
                    (_appSettingsOptions.Value.LogFolder + $@"/archetypedata.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        #endregion
    }
}
