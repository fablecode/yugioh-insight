using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using imageprocessor.application.Commands.DownloadImage;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace imageprocessor.application.MessageConsumers.CardImage
{
    public class CardImageConsumerHandler : IRequestHandler<CardImageConsumer, CardImageConsumerResult>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CardImageConsumerHandler> _logger;

        public CardImageConsumerHandler(IMediator mediator, ILogger<CardImageConsumerHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<CardImageConsumerResult> Handle(CardImageConsumer request, CancellationToken cancellationToken)
        {
            var cardImageConsumerResult = new CardImageConsumerResult();

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