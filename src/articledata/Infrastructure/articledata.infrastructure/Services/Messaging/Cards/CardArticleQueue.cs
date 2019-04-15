using System.Collections.Generic;
using articledata.domain.Contracts;
using articledata.domain.Services.Messaging.Cards;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.infrastructure.Services.Messaging.Cards
{
    public class CardArticleQueue : ICardArticleQueue
    {
        public Task Publish(UnexpandedArticle article)
        {
            var messageToBeSent = new SubmitArticle
            {
                Id = article.Id,
                Title = article.Title,
                Url = article.Url
            };

            var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageToBeSent));

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var props = channel.CreateBasicProperties();
                props.ContentType = "application/json";
                props.DeliveryMode = 2;

                props.Headers = new Dictionary<string, object>
                {
                    {"message-type", "cardarticle"},
                    { "x-match", "all"}
                };

                channel.BasicPublish
                (
                    "yugioh.headers.card",
                    "", 
                    props,
                    messageBodyBytes
                );
            }

            return Task.CompletedTask;
        }
    }
}