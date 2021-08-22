using MediatR;

namespace Cards.Application.MessageConsumers.CardData
{
    public record CardInformationConsumer : IRequest<CardDataConsumerResult>
    {
        public string Message { get; }

        public CardInformationConsumer(string message)
        {
            Message = message;
        }
    }
}