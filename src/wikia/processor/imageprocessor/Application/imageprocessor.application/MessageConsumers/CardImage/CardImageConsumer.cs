using MediatR;

namespace imageprocessor.application.MessageConsumers.CardImage
{
    public class CardImageConsumer : IRequest<CardImageConsumerResult>
    {
        public string Message { get; set; }
    }
}