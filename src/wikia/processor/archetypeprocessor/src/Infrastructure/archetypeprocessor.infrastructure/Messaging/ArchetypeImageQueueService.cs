using System.Threading.Tasks;
using archetypeprocessor.application.Configuration;
using archetypeprocessor.core.Models;
using archetypeprocessor.domain.Messaging;
using Microsoft.Extensions.Options;

namespace archetypeprocessor.infrastructure.Messaging
{
    public class ArchetypeImageQueueService : IArchetypeImageQueueService
    {
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IImageQueueService _imageQueueService;

        public ArchetypeImageQueueService(IOptions<AppSettings> appSettingsOptions, IImageQueueService imageQueueService)
        {
            _appSettingsOptions = appSettingsOptions;
            _imageQueueService = imageQueueService;
        }

        public async Task Publish(DownloadImage downloadImage)
        {
            downloadImage.ImageFolderPath = _appSettingsOptions.Value.ArchetypeImageFolderPath;

            await _imageQueueService.Publish(downloadImage);
        }
    }
}