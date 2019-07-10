using System;
using System.Threading.Tasks;
using article.application.Configuration;
using article.core.Models;
using article.domain.Services.Messaging;
using Microsoft.Extensions.Options;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.Services.Messaging
{
    public class ArchetypeArticleQueue : IArchetypeArticleQueue
    {
        private readonly IQueue<Article> _queue;
        private readonly IOptions<AppSettings> _appSettings;

        public ArchetypeArticleQueue(IQueue<Article> queue, IOptions<AppSettings> appSettings)
        {
            _queue = queue;
            _appSettings = appSettings;
        }

        public Task Publish(UnexpandedArticle article)
        {
            var messageToBeSent = new Article
            {
                Id = article.Id,
                CorrelationId = Guid.NewGuid(),
                Title = article.Title,
                Url = new Uri(new Uri(_appSettings.Value.WikiaDomainUrl), article.Url).AbsoluteUri
            };

            return _queue.Publish(messageToBeSent);
        }
    }
}