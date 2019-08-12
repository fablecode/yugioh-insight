using System.Threading;
using System.Threading.Tasks;
using cardsectionprocessor.application.MessageConsumers.CardInformation;
using cardsectionprocessor.core.Constants;
using MediatR;

namespace cardsectionprocessor.application.MessageConsumers.CardRulingInformation
{
    public class CardRulingInformationConsumerHandler : IRequestHandler<CardRulingInformationConsumer, CardRulingInformationConsumerResult>
    {
        private readonly IMediator _mediator;

        public CardRulingInformationConsumerHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<CardRulingInformationConsumerResult> Handle(CardRulingInformationConsumer request, CancellationToken cancellationToken)
        {
            var cardRulingInformationConsumerResult = new CardRulingInformationConsumerResult();

            var cardInformation = new CardInformationConsumer { Category = ArticleCategory.CardRulings, Message = request.Message };

            var result = await _mediator.Send(cardInformation, cancellationToken);

            if (!result.IsSuccessful)
            {
                cardRulingInformationConsumerResult.Errors = result.Errors;
            }

            return cardRulingInformationConsumerResult;
        }
    }
}