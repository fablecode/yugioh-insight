using MediatR;

namespace cardprocessor.application.MessageConsumers.CardData
{
    public class CardDataConsumer : IRequest<CardDataConsumerResult>
    {
        public string Message { get; set; }
    }
}