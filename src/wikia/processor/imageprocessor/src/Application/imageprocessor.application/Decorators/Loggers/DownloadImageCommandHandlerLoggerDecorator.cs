using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using imageprocessor.application.Commands;
using imageprocessor.application.Commands.DownloadImage;
using MediatR;
using Microsoft.Extensions.Logging;

namespace imageprocessor.application.Decorators.Loggers
{
    public class DownloadImageCommandHandlerLoggerDecorator
    {
        private readonly IRequestHandler<DownloadImageCommand, CommandResult> _requestHandler;
        private readonly ILogger<DownloadImageCommandHandler> _logger;

        public DownloadImageCommandHandlerLoggerDecorator(IRequestHandler<DownloadImageCommand, CommandResult> requestHandler, ILogger<DownloadImageCommandHandler> logger)
        {
            _requestHandler = requestHandler;
            _logger = logger;
        }

        public async Task<CommandResult> Handle(DownloadImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Downloading image '{@RemoteImageUrl}' to local path '{@LocalPath}'", request.RemoteImageUrl, Path.Combine(request.ImageFolderPath, request.ImageFileName));

                var commandResult = await _requestHandler.Handle(request, cancellationToken);

                _logger.LogInformation("Finished downloading image '{@RemoteImageUrl}' to local path '{@LocalPath}'", request.RemoteImageUrl, Path.Combine(request.ImageFolderPath, request.ImageFileName));

                return commandResult;
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error downloading image '{@RemoteImageUrl}' to local path '{@LocalPath}'. Exception: {@Exception}", request.RemoteImageUrl, Path.Combine(request.ImageFolderPath, request.ImageFileName), ex);
                return new CommandResult{ Errors = new List<string>{ ex.Message}};
            }
        }
    }
}