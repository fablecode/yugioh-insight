using cardprocessor.application.Configuration;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using cardprocessor.application.MessageConsumers.CardData;

namespace articledata.cardinformation.Services
{
    public class CardProcessorHostedService : IHostedService
    {
        private const string CardDataQueue = "card-data";
        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _options;
        private readonly IMediator _mediator;
        private readonly IHost _host;

        public CardProcessorHostedService
        (
            IServiceProvider services, 
            IOptions<RabbitMqSettings> options,
            IMediator mediator,
            IHost host
        )
        {
            Services = services;
            _options = options;
            _mediator = mediator;
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
                channel.BasicQos(0, 20, false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardDataConsumer { Message = message });

                    if(result.IsSuccessful)
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        channel.BasicNack(ea.DeliveryTag, false, false);
                    }
                };

                channel.BasicConsume
                (
                    queue: _options.Value.Queues[CardDataQueue].Name,
                    autoAck: _options.Value.Queues[CardDataQueue].AutoAck,
                    consumer: consumer
                );

                await _host.WaitForShutdownAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Hosted Service is stopping.");

            return Task.CompletedTask;
        }
    }
}
