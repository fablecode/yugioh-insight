using article.application.Configuration;
using article.core.Models;
using article.domain.Services.Messaging;
using article.domain.Services.Messaging.Cards;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.Services.Messaging.Cards
{
    public class SemanticCardArticleQueue : ISemanticCardArticleQueue
    {
        private readonly IOptions<RabbitMqSettings> _rabbitMqConfig;
        private readonly IQueue<Article> _queue;

        public SemanticCardArticleQueue(IOptions<RabbitMqSettings> rabbitMqConfig, IQueue<Article> queue)
        {
            _rabbitMqConfig = rabbitMqConfig;
            _queue = queue;
        }

        public Task Publish(UnexpandedArticle article)
        {
            var messageToBeSent = new Article
            {
                Id = article.Id,
                CorrelationId = Guid.NewGuid()
            };

            return _queue.Publish(messageToBeSent);
        }
    }
}