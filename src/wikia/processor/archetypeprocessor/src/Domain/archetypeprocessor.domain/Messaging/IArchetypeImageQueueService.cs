using System.Threading.Tasks;
using archetypeprocessor.core.Models;

namespace archetypeprocessor.domain.Messaging
{
    public interface IArchetypeImageQueueService
    {
        Task Publish(DownloadImage downloadImage);
    }
}