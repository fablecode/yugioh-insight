using System.Threading.Tasks;
using cardprocessor.core.Models;

namespace cardprocessor.core.Services.Messaging.Cards
{
    public interface ICardImageQueueService
    {
        Task<CardImageCompletion> Publish(DownloadImage downloadImage);
    }
}