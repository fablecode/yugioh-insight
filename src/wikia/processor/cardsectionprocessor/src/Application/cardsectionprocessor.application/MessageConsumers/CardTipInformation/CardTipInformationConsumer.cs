using MediatR;

namespace cardsectionprocessor.application.MessageConsumers.CardTipInformation
{
    public class CardTipInformationConsumer : IRequest<CardTipInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}