using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using banlistprocessor.application.Configuration;
using banlistprocessor.application.MessageConsumers.BanlistData;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace banlistprocessor.Services
{
    public class BanlistProcessorHostedService : BackgroundService
    {
        private const string BanlistDataQueue = "banlist-data";

        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _cardDataConsumer;

        public BanlistProcessorHostedService
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
            _connection.Close();
            _channel.Close();
            base.Dispose();
        }


        private Task StartConsumer()
        {
            _factory = new ConnectionFactory() {HostName = _rabbitMqOptions.Value.Host};
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, 1, false);

            _cardDataConsumer = new EventingBasicConsumer(_channel);

            _cardDataConsumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                var result = await _mediator.Send(new BanlistDataConsumer { Message = message });

                if(result.IsSuccessful)
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            _channel.BasicConsume
            (
                queue: _rabbitMqOptions.Value.Queues[BanlistDataQueue].Name,
                autoAck: _rabbitMqOptions.Value.Queues[BanlistDataQueue].AutoAck,
                consumer: _cardDataConsumer
            );

            return Task.CompletedTask;
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
                    (_appSettingsOptions.Value.LogFolder + $@"/banlistprocessor.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

    }
}
