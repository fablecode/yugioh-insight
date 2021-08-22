using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Cards.Application.MessageConsumers.CardData
{
    public sealed class CardInformationConsumerHandler : IRequestHandler<CardInformationConsumer, CardDataConsumerResult>
    {
        public Task<CardDataConsumerResult> Handle(CardInformationConsumer request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CardDataConsumerResult {IsSuccessful = true});
        }
    }
}