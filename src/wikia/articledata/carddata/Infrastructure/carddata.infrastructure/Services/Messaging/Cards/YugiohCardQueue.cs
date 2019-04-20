using System.Collections.Generic;
using System.Threading.Tasks;
using carddata.core.Models;
using carddata.domain.Services.Messaging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace carddata.infrastructure.Services.Messaging.Cards
{
    public class YugiohCardQueue : IYugiohCardQueue
    {
        public Task Publish(YugiohCard yugiohCard)
        {
            var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(yugiohCard));

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var props = channel.CreateBasicProperties();
                props.ContentType = "application/json";
                props.DeliveryMode = 2;

                props.Headers = new Dictionary<string, object>
                {
                    {"message-type", "carddata"},
                    { "x-match", "all"}
                };

                channel.BasicPublish
                (
                    "yugioh.headers.data",
                    "", 
                    props,
                    messageBodyBytes
                );
            }

            return Task.CompletedTask;
        }
    }
}