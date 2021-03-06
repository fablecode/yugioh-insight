﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using imageprocessor.application.Configuration;
using imageprocessor.application.MessageConsumers.YugiohImage;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace imageprocessor.Services
{
    public class ImageProcessorHostedService : BackgroundService
    {
        private const string CardImageQueue = "card-image";
        private const string ArchetypeImageQueue = "archetype-image";

        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IMediator _mediator;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _imageConsumer;

        public ImageProcessorHostedService
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

        private Task StartConsumer()
        {
            _factory = new ConnectionFactory() {HostName = _rabbitMqOptions.Value.Host};
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, 20, false);

            _imageConsumer = new EventingBasicConsumer(_channel);

            _imageConsumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                var result = await _mediator.Send(new ImageConsumer { Message = message });

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
                queue: _rabbitMqOptions.Value.Queues[CardImageQueue].Name,
                autoAck: _rabbitMqOptions.Value.Queues[CardImageQueue].AutoAck,
                consumer: _imageConsumer
            );

            _channel.BasicConsume
            (
                queue: _rabbitMqOptions.Value.Queues[ArchetypeImageQueue].Name,
                autoAck: _rabbitMqOptions.Value.Queues[ArchetypeImageQueue].AutoAck,
                consumer: _imageConsumer
            );

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _connection.Close();
            _channel.Close();
            base.Dispose();
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
                    (_appSettingsOptions.Value.LogFolder + $@"/imageprocessor.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

    }
}
