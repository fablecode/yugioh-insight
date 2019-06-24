using System;
using System.Threading.Tasks;
using article.core.Models;
using article.domain.Services;
using article.domain.Services.Messaging;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.Services.Messaging
{
    public class BanlistArticleQueue : IBanlistArticleQueue
    {
        private readonly IQueue<Article> _queue;

        public BanlistArticleQueue(IQueue<Article> queue)
        {
            _queue = queue;
        }

        public Task Publish(UnexpandedArticle article)
        {
            var messageToBeSent = new Article
            {
                Id = article.Id,
                CorrelationId = Guid.NewGuid(),
                Title = article.Title,
            };

            return _queue.Publish(messageToBeSent);
        }
    }
}