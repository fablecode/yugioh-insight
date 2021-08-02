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
    public class CardArticleQueue : ICardArticleQueue
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IQueue<Article> _queue;

        public CardArticleQueue(IOptions<AppSettings> appSettings, IQueue<Article> queue)
        {
            _appSettings = appSettings;
            _queue = queue;
        }

        public Task Publish(UnexpandedArticle article)
        {
            return _queue.Publish(article);
        }
    }
}