using cardsectiondata.application.MessageConsumers.CardInformation;
using cardsectiondata.core.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace cardsectiondata.application.MessageConsumers.CardRulingInformation
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