using cardsectiondata.application.MessageConsumers.CardInformation;
using cardsectiondata.core.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace cardsectiondata.application.MessageConsumers.CardTriviaInformation
{
    public class CardTriviaInformationConsumerHandler : IRequestHandler<CardTriviaInformationConsumer, CardTriviaInformationConsumerResult>
    {
        private readonly IMediator _mediator;

        public CardTriviaInformationConsumerHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CardTriviaInformationConsumerResult> Handle(CardTriviaInformationConsumer request, CancellationToken cancellationToken)
        {
            var cardTriviaInformationConsumerResult = new CardTriviaInformationConsumerResult();

            var cardInformation = new CardInformationConsumer { Category = ArticleCategory.CardTrivia, Message = request.Message };

            var result = await _mediator.Send(cardInformation, cancellationToken);

            if (!result.IsSuccessful)
            {
                cardTriviaInformationConsumerResult.Errors = result.Errors;
            }

            return cardTriviaInformationConsumerResult;
        }
    }
}