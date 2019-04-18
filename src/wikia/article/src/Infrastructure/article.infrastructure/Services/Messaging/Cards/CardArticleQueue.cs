using System.Collections.Generic;
using System.Threading.Tasks;
using article.core.Models;
using article.domain.Services.Messaging.Cards;
using Newtonsoft.Json;
using RabbitMQ.Client;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.Services.Messaging.Cards
{
    public class CardArticleQueue : ICardArticleQueue
    {
        public Task Publish(UnexpandedArticle article)
        {
            var messageToBeSent = new Article
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
                    //{ "x-match", "all"}
                };

                channel.BasicPublish
                (
                    "yugioh.headers.article",
                    "", 
                    props,
                    messageBodyBytes
                );
            }

            return Task.CompletedTask;
        }
    }
}