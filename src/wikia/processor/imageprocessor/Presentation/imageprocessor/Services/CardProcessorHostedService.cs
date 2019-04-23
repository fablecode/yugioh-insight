using imageprocessor.application.Configuration;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using imageprocessor.application.MessageConsumers.CardImage;

namespace articledata.cardinformation.Services
{
    public class ImageProcessorHostedService : IHostedService
    {
        public IServiceProvider Services { get; }

        private readonly IOptions<RabbitMqSettings> _options;
        private readonly IMediator _mediator;
        private readonly IHost _host;

        public ImageProcessorHostedService
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
                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var result = await _mediator.Send(new CardImageConsumer { Message = message });

                    if(result.IsSuccessful)
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        channel.BasicNack(ea.DeliveryTag, false, false);
                    }
                };

                channel.BasicConsume(queue: "card-image",
                    autoAck: false,
                    consumer: consumer);

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
