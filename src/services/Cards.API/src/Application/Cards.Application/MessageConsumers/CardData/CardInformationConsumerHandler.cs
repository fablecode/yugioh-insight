using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Cards.Application.MessageConsumers.CardData
{
    public sealed class CardInformationConsumerHandler : IRequestHandler<CardInformationConsumer, CardInformationConsumerResult>
    {
        public Task<CardInformationConsumerResult> Handle(CardInformationConsumer request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CardInformationConsumerResult {IsSuccessful = true});
        }
    }
}