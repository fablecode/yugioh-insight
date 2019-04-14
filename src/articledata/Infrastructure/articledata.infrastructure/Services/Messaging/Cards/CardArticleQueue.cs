using articledata.domain.Contracts;
using articledata.domain.Services.Messaging.Cards;
using MassTransit;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.infrastructure.Services.Messaging.Cards
{
    public class CardArticleQueue : ICardArticleQueue
    {
        private readonly IBus _bus;

        public CardArticleQueue(IBus bus)
        {
            _bus = bus;
        }

        public async Task Publish(UnexpandedArticle article)
        {
            var messageToBeSent = new SubmitArticle
            {
                Id = article.Id,
                Title = article.Title,
                Url = article.Url
            };

            await _bus.Publish(messageToBeSent);
        }
    }
}