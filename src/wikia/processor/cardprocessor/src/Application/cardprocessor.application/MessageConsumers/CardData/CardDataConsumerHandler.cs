using cardprocessor.application.Commands;
using cardprocessor.core.Models;
using cardprocessor.core.Services;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace cardprocessor.application.MessageConsumers.CardData
{
    public class CardDataConsumerHandler : IRequestHandler<CardDataConsumer, CardDataConsumerResult>
    {
        private readonly ICardService _cardService;
        private readonly IMediator _mediator;
        private readonly ICardCommandMapper _cardCommandMapper;
        private readonly ILogger<CardDataConsumerHandler> _logger;

        public CardDataConsumerHandler(ICardService cardService, IMediator mediator, ICardCommandMapper cardCommandMapper, ILogger<CardDataConsumerHandler> logger)
        {
            _cardService = cardService;
            _mediator = mediator;
            _cardCommandMapper = cardCommandMapper;
            _logger = logger;
        }
        public async Task<CardDataConsumerResult> Handle(CardDataConsumer request, CancellationToken cancellationToken)
        {
            var cardDataConsumerResult = new CardDataConsumerResult();
            var yugiohCard = JsonConvert.DeserializeObject<YugiohCard>(request.Message);

            try
            {
                _logger.LogInformation($"{yugiohCard.Name} processing...");

                var existingCard = await _cardService.CardByName(yugiohCard.Name);

                var result = existingCard == null
                    ? await _mediator.Send(await _cardCommandMapper.MapToAddCommand(yugiohCard), cancellationToken)
                    : await _mediator.Send(await _cardCommandMapper.MapToUpdateCommand(yugiohCard, existingCard), cancellationToken);

                cardDataConsumerResult.IsSuccessful = result.IsSuccessful;
                _logger.LogInformation($"{yugiohCard.Name} processed successfully.");

            }
            catch (System.Exception ex)
            {
                cardDataConsumerResult.Exception = ex;
                _logger.LogError(yugiohCard.Name + " error. Exception: {@Exception}", ex);
            }

            return cardDataConsumerResult;
        }
    }
}