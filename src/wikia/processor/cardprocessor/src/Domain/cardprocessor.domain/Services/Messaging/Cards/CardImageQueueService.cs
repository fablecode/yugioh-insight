using System.Threading.Tasks;
using cardprocessor.core.Models;
using cardprocessor.core.Services.Messaging.Cards;
using cardprocessor.domain.Queues.Cards;

namespace cardprocessor.domain.Services.Messaging.Cards
{
    public class CardImageQueueService : ICardImageQueueService
    {
        private readonly ICardImageQueue _cardImageQueue;

        public CardImageQueueService(ICardImageQueue cardImageQueue)
        {
            _cardImageQueue = cardImageQueue;
        }
        public Task<CardImageCompletion> Publish(DownloadImage downloadImage)
        {
            return _cardImageQueue.Publish(downloadImage);
        }
    }
}