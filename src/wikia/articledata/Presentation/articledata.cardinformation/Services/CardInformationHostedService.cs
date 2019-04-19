using articledata.application.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace articledata.cardinformation.Services
{
    public class CardInformationHostedService : IHostedService
    {
        public IServiceProvider Services { get; }

        private readonly ILogger<CardInformationHostedService> _logger;
        private readonly IOptions<RabbitMqSettings> _options;
        private readonly IHost _host;

        public CardInformationHostedService
        (
            IServiceProvider services, 
            ILogger<CardInformationHostedService> logger, 
            IOptions<RabbitMqSettings> options,
            IHost host
        )
        {
            Services = services;
            _logger = logger;
            _options = options;
            _host = host;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await StartConsumer();
        }

        private async Task StartConsumer()
        {
            var factory = new ConnectionFactory() {HostName = _options.Value.Host};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };

                channel.BasicConsume(queue: "card-article",
                    autoAck: true,
                    consumer: consumer);

                await _host.WaitForShutdownAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");

            return Task.CompletedTask;
        }
    }
}
