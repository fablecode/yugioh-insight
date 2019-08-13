using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using cardsectionprocessor.application.Configuration;
using cardsectionprocessor.application.MessageConsumers.CardRulingInformation;
using cardsectionprocessor.application.MessageConsumers.CardTipInformation;
using cardsectionprocessor.application.MessageConsumers.CardTriviaInformation;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace cardsectionprocessor.Services
{
    public class CardSectionProcessorHostedService : BackgroundService
    {
        private const string CardTipDataQueue = "card-tips-data";
        private const string CardRulingDataQueue = "card-rulings-data";
        private const string CardTriviaDataQueue = "card-trivia-data";

        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;

        private ConnectionFactory _factory;
        private IConnection _cardTipConnection;
        private IModel _cardTipDataChannel;

        private EventingBasicConsumer _cardTipDataConsumer;
        private EventingBasicConsumer _cardRulingDataConsumer;
        private IModel _cardRulingDataChannel;
        private IConnection _cardRulingConnection;
        private IConnection _cardTriviaConnection;
        private EventingBasicConsumer _cardTriviaDataConsumer;
        private IModel _cardTriviaDataChannel;

        public CardSectionProcessorHostedService
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
            _cardTipDataChannel.Close();
            _cardTipConnection.Close();

            _cardRulingDataChannel.Close();
            _cardRulingConnection.Close();

            _cardTriviaDataChannel.Close();
            _cardTriviaConnection.Close();

            base.Dispose();
        }

        #region private helper 

        private Task StartConsumer()
        {
            _factory = new ConnectionFactory() { HostName = _rabbitMqOptions.Value.Host };

            _cardTipConnection = _factory.CreateConnection();
            _cardRulingConnection = _factory.CreateConnection();
            _cardTriviaConnection = _factory.CreateConnection();

            _cardTipDataConsumer = CreateCardTipDataConsumer(_cardTipConnection);
            _cardRulingDataConsumer = CreateCardRulingDataConsumer(_cardRulingConnection);
            _cardTriviaDataConsumer = CreateCardTriviaDataConsumer(_cardTriviaConnection);

            return Task.CompletedTask;
        }

        private EventingBasicConsumer CreateCardTriviaDataConsumer(IConnection connection)
        {
            _cardTriviaDataChannel = connection.CreateModel();
            _cardTriviaDataChannel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_cardTriviaDataChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardTriviaInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _cardTriviaDataChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _cardTriviaDataChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + CardTriviaDataQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnCardTriviaDataConsumerShutdown;
            consumer.Registered += OnCardTriviaDataConsumerRegistered;
            consumer.Unregistered += OnCardTriviaDataConsumerUnregistered;
            consumer.ConsumerCancelled += OnCardTriviaDataConsumerCancelled;

            _cardTriviaDataChannel.BasicConsume
            (
                queue: CardTriviaDataQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
        }
        private EventingBasicConsumer CreateCardTipDataConsumer(IConnection connection)
        {
            _cardTipDataChannel = connection.CreateModel();
            _cardTipDataChannel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_cardTipDataChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardTipInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _cardTipDataChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _cardTipDataChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + CardTipDataQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnCardTipDataConsumerShutdown;
            consumer.Registered += OnCardTipDataConsumerRegistered;
            consumer.Unregistered += OnCardTipDataConsumerUnregistered;
            consumer.ConsumerCancelled += OnCardTipDataConsumerCancelled;

            _cardTipDataChannel.BasicConsume
            (
                queue: CardTipDataQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
        }
        private EventingBasicConsumer CreateCardRulingDataConsumer(IConnection connection)
        {
            _cardRulingDataChannel = connection.CreateModel();
            _cardRulingDataChannel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_cardRulingDataChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardRulingInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _cardRulingDataChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _cardRulingDataChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + CardRulingDataQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnCardRulingDataConsumerShutdown;
            consumer.Registered += OnCardRulingDataConsumerRegistered;
            consumer.Unregistered += OnCardRulingDataConsumerUnregistered;
            consumer.ConsumerCancelled += OnCardRulingDataConsumerCancelled;

            _cardRulingDataChannel.BasicConsume
            (
                queue: CardRulingDataQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
        }


        private static void OnCardTipDataConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTipDataQueue}' Cancelled");
        }
        private static void OnCardTipDataConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTipDataQueue}' Unregistered");}
        private static void OnCardTipDataConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTipDataQueue}' Registered");
        }
        private static void OnCardTipDataConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTipDataQueue}' Shutdown");
        }



        private static void OnCardRulingDataConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardRulingDataQueue}' Cancelled");
        }
        private static void OnCardRulingDataConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardRulingDataQueue}' Unregistered");}
        private static void OnCardRulingDataConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardRulingDataQueue}' Registered");
        }
        private static void OnCardRulingDataConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardRulingDataQueue}' Shutdown");
        }


        private static void OnCardTriviaDataConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTriviaDataQueue}' Cancelled");
        }
        private static void OnCardTriviaDataConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTriviaDataQueue}' Unregistered");}
        private static void OnCardTriviaDataConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTriviaDataQueue}' Registered");
        }
        private static void OnCardTriviaDataConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTriviaDataQueue}' Shutdown");
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
                    (_appSettingsOptions.Value.LogFolder + $@"/cardsectionprocessor.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        #endregion
    }
}
