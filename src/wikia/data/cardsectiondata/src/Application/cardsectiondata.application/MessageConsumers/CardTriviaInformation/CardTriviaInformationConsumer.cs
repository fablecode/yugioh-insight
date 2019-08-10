using MediatR;

namespace cardsectiondata.application.MessageConsumers.CardTriviaInformation
{
    public class CardTriviaInformationConsumer : IRequest<CardTriviaInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}