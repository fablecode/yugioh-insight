using MediatR;

namespace cardsectiondata.application.MessageConsumers.CardTipInformation
{
    public class CardTipInformationConsumer : IRequest<CardTipInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}