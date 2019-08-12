using MediatR;

namespace cardsectionprocessor.application.MessageConsumers.CardTriviaInformation
{
    public class CardTriviaInformationConsumer : IRequest<CardTriviaInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}