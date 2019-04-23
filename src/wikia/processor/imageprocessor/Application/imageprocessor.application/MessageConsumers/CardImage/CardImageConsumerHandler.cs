using System;
using System.Threading;
using System.Threading.Tasks;
using imageprocessor.application.Commands.DownloadImage;
using MediatR;
using Newtonsoft.Json;

namespace imageprocessor.application.MessageConsumers.CardImage
{
    public class CardImageConsumerHandler : IRequestHandler<CardImageConsumer, CardImageConsumerResult>
    {
        private readonly IMediator _mediator;

        public CardImageConsumerHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CardImageConsumerResult> Handle(CardImageConsumer request, CancellationToken cancellationToken)
        {
            var cardImageConsumerResult = new CardImageConsumerResult();

            try
            {
                var downloadCommand = JsonConvert.DeserializeObject<DownloadImageCommand>(request.Message);

                var result = await _mediator.Send(downloadCommand, cancellationToken);

                cardImageConsumerResult.IsSuccessful = result.IsSuccessful;
            }
            catch (Exception ex)
            {
                cardImageConsumerResult.Exception = ex;
            }

            return cardImageConsumerResult;
        }
    }
}