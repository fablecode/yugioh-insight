using MediatR;

namespace archetypeprocessor.application.MessageConsumers.ArchetypeCardInformation
{
    public class ArchetypeCardInformationConsumer : IRequest<ArchetypeCardInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}