using System.Threading.Tasks;
using archetypeprocessor.core.Models;

namespace archetypeprocessor.domain.Messaging
{
    public interface IImageQueueService
    {
        Task Publish(DownloadImage downloadImage);
    }
}