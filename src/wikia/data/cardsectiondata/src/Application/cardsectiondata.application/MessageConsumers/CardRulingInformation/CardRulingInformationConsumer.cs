using MediatR;

namespace cardsectiondata.application.MessageConsumers.CardRulingInformation
{
    public class CardRulingInformationConsumer : IRequest<CardRulingInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}