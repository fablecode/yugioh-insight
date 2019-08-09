using System.Threading.Tasks;
using cardsectiondata.core.Constants;
using cardsectiondata.core.Models;
using cardsectiondata.domain.Services.Messaging;

namespace cardsectiondata.infrastructure.Services.Messaging
{
    public class CardTipsQueue : IQueue
    {
        public Task Publish(CardSectionMessage message)
        {
            throw new System.NotImplementedException();
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardTips;
        }
    }
}