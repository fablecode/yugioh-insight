using cardsectiondata.application.Configuration;
using cardsectiondata.application.MessageConsumers.CardRulingInformation;
using cardsectiondata.application.MessageConsumers.CardTipInformation;
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
using cardsectiondata.application.MessageConsumers.CardTriviaInformation;

namespace cardsectiondata.Services
{
    public class CardSectionDataHostedService : BackgroundService
    {
        private const string CardTipArticleQueue = "card-tips-article";
        private const string CardRulingArticleQueue = "card-rulings-article";
        private const string CardTriviaArticleQueue = "card-trivia-article";

        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;

        private ConnectionFactory _factory;
        private IConnection _cardTipConnection;
        private IModel _cardTipArticleChannel;

        private EventingBasicConsumer _cardTipArticleConsumer;
        private EventingBasicConsumer _cardRulingArticleConsumer;
        private IModel _cardRulingArticleChannel;
        private IConnection _cardRulingConnection;
        private IConnection _cardTriviaConnection;
        private EventingBasicConsumer _cardTriviaArticleConsumer;
        private IModel _cardTriviaArticleChannel;

        public CardSectionDataHostedService
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
            _cardTipArticleChannel.Close();
            _cardTipConnection.Close();

            _cardRulingArticleChannel.Close();
            _cardRulingConnection.Close();

            _cardTriviaArticleChannel.Close();
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

            _cardTipArticleConsumer = CreateCardTipArticleConsumer(_cardTipConnection);
            _cardRulingArticleConsumer = CreateCardRulingArticleConsumer(_cardRulingConnection);
            _cardTriviaArticleConsumer = CreateCardTriviaArticleConsumer(_cardTriviaConnection);

            return Task.CompletedTask;
        }

        private EventingBasicConsumer CreateCardTriviaArticleConsumer(IConnection connection)
        {
            _cardTriviaArticleChannel = connection.CreateModel();
            _cardTriviaArticleChannel.BasicQos(0, 20, false);

            var consumer = new EventingBasicConsumer(_cardTriviaArticleChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardRulingInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _cardTriviaArticleChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _cardTriviaArticleChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + CardTriviaArticleQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnCardTriviaArticleConsumerShutdown;
            consumer.Registered += OnCardTriviaArticleConsumerRegistered;
            consumer.Unregistered += OnCardTriviaArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnCardTriviaArticleConsumerCancelled;

            _cardTriviaArticleChannel.BasicConsume
            (
                queue: CardTriviaArticleQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
        }
        private EventingBasicConsumer CreateCardTipArticleConsumer(IConnection connection)
        {
            _cardTipArticleChannel = connection.CreateModel();
            _cardTipArticleChannel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_cardTipArticleChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardTipInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _cardTipArticleChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _cardTipArticleChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + CardTipArticleQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnCardTipArticleConsumerShutdown;
            consumer.Registered += OnCardTipArticleConsumerRegistered;
            consumer.Unregistered += OnCardTipArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnCardTipArticleConsumerCancelled;

            _cardTipArticleChannel.BasicConsume
            (
                queue: CardTipArticleQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
        }
        private EventingBasicConsumer CreateCardRulingArticleConsumer(IConnection connection)
        {
            _cardRulingArticleChannel = connection.CreateModel();
            _cardRulingArticleChannel.BasicQos(0, 20, false);

            var consumer = new EventingBasicConsumer(_cardRulingArticleChannel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardTriviaInformationConsumer { Message = message });


                    if (result.IsSuccessful)
                    {
                        _cardRulingArticleChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _cardRulingArticleChannel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("RabbitMq Consumer: " + CardRulingArticleQueue + " exception. Exception: {@Exception}", ex);
                }
            };

            consumer.Shutdown += OnCardRulingArticleConsumerShutdown;
            consumer.Registered += OnCardRulingArticleConsumerRegistered;
            consumer.Unregistered += OnCardRulingArticleConsumerUnregistered;
            consumer.ConsumerCancelled += OnCardRulingArticleConsumerCancelled;

            _cardRulingArticleChannel.BasicConsume
            (
                queue: CardRulingArticleQueue,
                autoAck: false,
                consumer: consumer
            );

            return consumer;
        }


        private static void OnCardTipArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTipArticleQueue}' Cancelled");
        }
        private static void OnCardTipArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTipArticleQueue}' Unregistered");}
        private static void OnCardTipArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTipArticleQueue}' Registered");
        }
        private static void OnCardTipArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTipArticleQueue}' Shutdown");
        }



        private static void OnCardRulingArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardRulingArticleQueue}' Cancelled");
        }
        private static void OnCardRulingArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardRulingArticleQueue}' Unregistered");}
        private static void OnCardRulingArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardRulingArticleQueue}' Registered");
        }
        private static void OnCardRulingArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardRulingArticleQueue}' Shutdown");
        }


        private static void OnCardTriviaArticleConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTriviaArticleQueue}' Cancelled");
        }
        private static void OnCardTriviaArticleConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTriviaArticleQueue}' Unregistered");}
        private static void OnCardTriviaArticleConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTriviaArticleQueue}' Registered");
        }
        private static void OnCardTriviaArticleConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Logger.Information($"Consumer '{CardTriviaArticleQueue}' Shutdown");
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
