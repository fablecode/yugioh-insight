using cardprocessor.application.Commands;
using cardprocessor.core.Models;
using cardprocessor.core.Services;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace cardprocessor.application.MessageConsumers.CardData
{
    public class CardDataConsumerHandler : IRequestHandler<CardDataConsumer, CardDataConsumerResult>
    {
        private readonly ICardService _cardService;
        private readonly IMediator _mediator;
        private readonly ICardCommandMapper _cardCommandMapper;

        public CardDataConsumerHandler(ICardService cardService, IMediator mediator, ICardCommandMapper cardCommandMapper)
        {
            _cardService = cardService;
            _mediator = mediator;
            _cardCommandMapper = cardCommandMapper;
        }
        public async Task<CardDataConsumerResult> Handle(CardDataConsumer request, CancellationToken cancellationToken)
        {
            var cardDataConsumerResult = new CardDataConsumerResult();

            try
            {
                var yugiohCard = JsonConvert.DeserializeObject<YugiohCard>(request.Message);

                var existingCard = await _cardService.CardByName(yugiohCard.Name);

                var result = existingCard == null
                    ? await _mediator.Send(await _cardCommandMapper.MapToAddCommand(yugiohCard), cancellationToken)
                    : await _mediator.Send(await _cardCommandMapper.MapToUpdateCommand(yugiohCard, existingCard), cancellationToken);

                cardDataConsumerResult.IsSuccessful = result.IsSuccessful;
            }
            catch (System.Exception ex)
            {
                cardDataConsumerResult.Exception = ex;
            }

            return cardDataConsumerResult;
        }
    }
}