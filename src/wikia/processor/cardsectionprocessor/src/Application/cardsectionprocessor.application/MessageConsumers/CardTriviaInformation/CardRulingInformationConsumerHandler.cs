using System.Threading;
using System.Threading.Tasks;
using cardsectionprocessor.application.MessageConsumers.CardInformation;
using cardsectionprocessor.core.Constants;
using MediatR;

namespace cardsectionprocessor.application.MessageConsumers.CardTriviaInformation
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