using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using imageprocessor.application.Commands.DownloadImage;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace imageprocessor.application.MessageConsumers.YugiohImage
{
    public class ImageConsumerHandler : IRequestHandler<ImageConsumer, ImageConsumerResult>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ImageConsumerHandler> _logger;

        public ImageConsumerHandler(IMediator mediator, ILogger<ImageConsumerHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ImageConsumerResult> Handle(ImageConsumer request, CancellationToken cancellationToken)
        {
            var cardImageConsumerResult = new ImageConsumerResult();

            try
            {
                var downloadCommand = JsonConvert.DeserializeObject<DownloadImageCommand>(request.Message);

                _logger.LogInformation("Downloading image '{@RemoteImageUrl}' to local path '{@LocalPath}'", downloadCommand.RemoteImageUrl, Path.Combine(downloadCommand.ImageFolderPath, downloadCommand.ImageFileName));
                var result = await _mediator.Send(downloadCommand, cancellationToken);
                _logger.LogInformation("Finished downloading image '{@RemoteImageUrl}' to local path '{@LocalPath}'", downloadCommand.RemoteImageUrl, Path.Combine(downloadCommand.ImageFolderPath, downloadCommand.ImageFileName));

                cardImageConsumerResult.IsSuccessful = result.IsSuccessful;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error downloading image '{@MessageJson}'. Exception: {@Exception}", request.Message, ex);
                cardImageConsumerResult.Exception = ex;
            }

            return cardImageConsumerResult;
        }
    }
}