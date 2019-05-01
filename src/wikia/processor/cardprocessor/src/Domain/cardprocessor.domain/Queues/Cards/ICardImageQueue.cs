using System.Threading.Tasks;
using cardprocessor.core.Models;

namespace cardprocessor.domain.Queues.Cards
{
    public interface ICardImageQueue
    {
        Task<CardImageCompletion> Publish(DownloadImage downloadImage);
    }
}