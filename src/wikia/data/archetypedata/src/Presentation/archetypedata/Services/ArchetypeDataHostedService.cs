using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using archetypedata.application.Configuration;
using archetypedata.application.MessageConsumers.ArchetypeCardInformation;
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
        private const string ArchetypeCardArticleQueue = "archetype-cards-article";
        private const string ArchetypeSupportCardArticleQueue = "archetype-support-cards-article";

        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;

        private ConnectionFactory _factory;
        private IConnection _archetypeConnection;
        private IModel _archetypeArticleChannel;

        private EventingBasicConsumer _archetypeArticleConsumer;
        private EventingBasicConsumer _archetypeCardArticleConsumer;
        private IModel _archetypeCardArticleChannel;
        private IConnection _archetypeCardConnection;

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
            _archetypeConnection = _factory.CreateConnection();
            _archetypeCardConnection = _factory.CreateConnection();

            _archetypeArticleConsumer = CreateArchetypeArticleConsumer(_archetypeConnection);
            _archetypeCardArticleConsumer = CreateArchetypeCardArticleConsumer(_archetypeCardConnection);

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
        private EventingBasicConsumer CreateArchetypeCardArticleConsumer(IConnection connection)
        {
            _archetypeCardArticleChannel = connection.CreateModel();
            _archetypeCardArticleChannel.BasicQos(0, 20, false);

            var consumer = new EventingBasicConsumer(_archetypeCardArticleChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new ArchetypeCardInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _archetypeCardArticleChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _archetypeCardArticleChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + ArchetypeCardArticleQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnArchetypeCardArticleConsumerShutdown;
            consumer.Registered += OnArchetypeCardArticleConsumerRegistered;
            consumer.Unregistered += OnArchetypeCardArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnArchetypeCardArticleConsumerCancelled;

            _archetypeCardArticleChannel.BasicConsume
            (
                queue: ArchetypeCardArticleQueue,
                autoAck: false,
                consumer: consumer
            );

            _archetypeCardArticleChannel.BasicConsume
            (
                queue: ArchetypeSupportCardArticleQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
        }

        public override void Dispose()
        {
            _archetypeArticleChannel.Close();
            _archetypeConnection.Close();
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



        private static void OnArchetypeCardArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeCardArticleQueue}' Cancelled");
        }

        private static void OnArchetypeCardArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeCardArticleQueue}' Unregistered");}

        private static void OnArchetypeCardArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeCardArticleQueue}' Registered");
        }
        private static void OnArchetypeCardArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeCardArticleQueue}' Shutdown");
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
