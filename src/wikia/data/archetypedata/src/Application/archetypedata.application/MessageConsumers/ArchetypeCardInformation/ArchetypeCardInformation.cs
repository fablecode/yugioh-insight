using MediatR;

namespace archetypedata.application.MessageConsumers.ArchetypeCardInformation
{
    public class ArchetypeCardInformationConsumer : IRequest<ArchetypeCardInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}