using cardprocessor.core.Services.Messaging.Cards;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cardprocessor.application.Commands.DownloadImage
{
    public class DownloadImageCommandHandler : IRequestHandler<DownloadImageCommand, CommandResult>
    {
        private readonly ICardImageQueueService _cardImageQueueService;
        private readonly IValidator<DownloadImageCommand> _validator;

        public DownloadImageCommandHandler(ICardImageQueueService cardImageQueueService, IValidator<DownloadImageCommand> validator)
        {
            _cardImageQueueService = cardImageQueueService;
            _validator = validator;
        }

        public async Task<CommandResult> Handle(DownloadImageCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var validationResult = _validator.Validate(request);

            if (validationResult.IsValid)
            {
                var result = await _cardImageQueueService.Publish(new core.Models.DownloadImage
                {
                    RemoteImageUrl = request.RemoteImageUrl,
                    ImageFileName = request.ImageFileName,
                    ImageFolderPath = request.ImageFolderPath
                });

                commandResult.IsSuccessful = result.IsSuccessful;
            }
            else
            {
                commandResult.Errors = validationResult.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return commandResult;
        }

    }
}