using AutoMapper;
using cardprocessor.application.Commands.DownloadImage;
using cardprocessor.application.Configuration;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;
using cardprocessor.core.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cardprocessor.application.Commands.UpdateCard
{
    public class UpdateCardCommandHandler : IRequestHandler<UpdateCardCommand, CommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CardInputModel> _validator;
        private readonly ICardService _cardService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IMapper _mapper;

        public UpdateCardCommandHandler
        (
            IMediator mediator, 
            IValidator<CardInputModel> validator,
            ICardService cardService,
            IOptions<AppSettings> settings,
            IMapper mapper
        )
        {
            _mediator = mediator;
            _validator = validator;
            _cardService = cardService;
            _settings = settings;
            _mapper = mapper;
        }

        public async Task<CommandResult> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var validationResults = _validator.Validate(request.Card);

            if (validationResults.IsValid)
            {
                var cardModel = _mapper.Map<CardModel>(request.Card);

                var cardUpdated = await _cardService.Update(cardModel);

                if (cardUpdated != null)
                {
                    if (request.Card.ImageUrl != null)
                    {
                        var downloadImageCommand = new DownloadImageCommand
                        {
                            RemoteImageUrl = request.Card.ImageUrl,
                            ImageFileName = request.Card.Name,
                            ImageFolderPath = _settings.Value.CardImageFolderPath
                        };

                        await _mediator.Send(downloadImageCommand, cancellationToken);
                    }

                    commandResult.Data = CommandMapperHelper.MapCardByCardType(_mapper, request.Card.CardType.GetValueOrDefault(), cardUpdated);
                    commandResult.IsSuccessful = true;
                }
                else
                {
                    commandResult.Errors = new List<string> { "Card not updated in data source." };
                }
            }
            else
            {
                commandResult.Errors = validationResults.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return commandResult;
        }

    }
}