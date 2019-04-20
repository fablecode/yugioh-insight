using MediatR;

namespace carddata.application.MessageConsumers.CardInformation
{
    public class CardInformationConsumer : IRequest<CardInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}