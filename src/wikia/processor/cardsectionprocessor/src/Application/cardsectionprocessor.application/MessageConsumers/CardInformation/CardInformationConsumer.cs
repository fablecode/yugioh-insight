using MediatR;

namespace cardsectionprocessor.application.MessageConsumers.CardInformation
{
    public class CardInformationConsumer : IRequest<CardInformationConsumerResult>
    {
        public string Category { get; set; }
        public string Message { get; set; }
    }
}