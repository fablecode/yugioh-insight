using MediatR;

namespace cardsectionprocessor.application.MessageConsumers.CardRulingInformation
{
    public class CardRulingInformationConsumer : IRequest<CardRulingInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}