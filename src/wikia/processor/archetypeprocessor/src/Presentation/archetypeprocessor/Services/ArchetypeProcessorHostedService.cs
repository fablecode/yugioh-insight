using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using archetypeprocessor.application.Configuration;
using archetypeprocessor.application.MessageConsumers.ArchetypeCardInformation;
using archetypeprocessor.application.MessageConsumers.ArchetypeInformation;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace archetypeprocessor.Services
{
    public class ArchetypeProcessorHostedService : BackgroundService
    {
        private const string ArchetypeDataQueue = "archetype-data";
        private const string ArchetypeCardDataQueue = "archetype-cards-data";

        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;
        private ConnectionFactory _factory;
        private IModel _archetypeDataChannel;
        private IModel _archetypeCardDataChannel;
        private IConnection _archetypeConnection;
        private IConnection _archetypeCardConnection;
        private EventingBasicConsumer _archetypeArticleConsumer;
        private EventingBasicConsumer _archetypeCardArticleConsumer;

        public ArchetypeProcessorHostedService
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

        public override void Dispose()
        {
            _archetypeConnection.Close();
            _archetypeCardConnection.Close();

            _archetypeDataChannel.Close();
            _archetypeCardDataChannel.Close();

            base.Dispose();
        }


        private Task StartConsumer()
        {
            _factory = new ConnectionFactory() { HostName = _rabbitMqOptions.Value.Host };
            _archetypeConnection = _factory.CreateConnection();
            _archetypeCardConnection = _factory.CreateConnection();

            _archetypeArticleConsumer = CreateArchetypeDataConsumer(_archetypeConnection);
            _archetypeCardArticleConsumer = CreateArchetypeCardDataConsumer(_archetypeCardConnection);

            return Task.CompletedTask;
        }

        private EventingBasicConsumer CreateArchetypeDataConsumer(IConnection connection)
        {
            _archetypeDataChannel = connection.CreateModel();
            _archetypeDataChannel.BasicQos(0, 20, false);

            var consumer = new EventingBasicConsumer(_archetypeDataChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new ArchetypeInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _archetypeDataChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _archetypeDataChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + ArchetypeDataQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnArchetypeDataConsumerShutdown;
            consumer.Registered += OnArchetypeDataConsumerRegistered;
            consumer.Unregistered += OnArchetypeDataConsumerUnregistered;
            consumer.ConsumerCancelled += OnArchetypeDataConsumerCancelled;

            _archetypeDataChannel.BasicConsume
            (
                queue: ArchetypeDataQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
        }

        private EventingBasicConsumer CreateArchetypeCardDataConsumer(IConnection connection)
        {
            _archetypeCardDataChannel = connection.CreateModel();
            _archetypeCardDataChannel.BasicQos(0, 20, false);

            var consumer = new EventingBasicConsumer(_archetypeCardDataChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new ArchetypeCardInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _archetypeCardDataChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _archetypeCardDataChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + ArchetypeCardDataQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnArchetypeCardArticleConsumerShutdown;
            consumer.Registered += OnArchetypeCardArticleConsumerRegistered;
            consumer.Unregistered += OnArchetypeCardArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnArchetypeCardArticleConsumerCancelled;

            _archetypeCardDataChannel.BasicConsume
            (
                queue: ArchetypeCardDataQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
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
                    (_appSettingsOptions.Value.LogFolder + $@"/archetypeprocessor.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        private static void OnArchetypeDataConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeDataQueue}' Cancelled");
        }

        private static void OnArchetypeDataConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeDataQueue}' Unregistered");
        }

        private static void OnArchetypeDataConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeDataQueue}' Registered");
        }
        private static void OnArchetypeDataConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeDataQueue}' Shutdown");
        }


        private static void OnArchetypeCardArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeCardDataQueue}' Cancelled");
        }

        private static void OnArchetypeCardArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeCardDataQueue}' Unregistered");
        }

        private static void OnArchetypeCardArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeCardDataQueue}' Registered");
        }
        private static void OnArchetypeCardArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{ArchetypeCardDataQueue}' Shutdown");
        }

    }
}
