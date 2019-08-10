using cardsectiondata.application.MessageConsumers.CardInformation;
using cardsectiondata.core.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace cardsectiondata.application.MessageConsumers.CardTipInformation
{
    public class CardTipInformationConsumerHandler : IRequestHandler<CardTipInformationConsumer, CardTipInformationConsumerResult>
    {
        private readonly IMediator _mediator;

        public CardTipInformationConsumerHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CardTipInformationConsumerResult> Handle(CardTipInformationConsumer request, CancellationToken cancellationToken)
        {
            var cardTipInformationConsumerResult = new CardTipInformationConsumerResult();

            var cardInformation = new CardInformationConsumer { Category = ArticleCategory.CardTips, Message = request.Message};

            var result = await _mediator.Send(cardInformation, cancellationToken);

            if (!result.IsSuccessful)
            {
                cardTipInformationConsumerResult.Errors = result.Errors;
            }

            return cardTipInformationConsumerResult;
        }
    }
}