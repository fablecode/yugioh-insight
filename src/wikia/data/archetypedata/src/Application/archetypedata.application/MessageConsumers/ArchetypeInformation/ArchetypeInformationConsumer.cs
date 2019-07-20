using MediatR;

namespace archetypedata.application.MessageConsumers.ArchetypeInformation
{
    public class ArchetypeInformationConsumer : IRequest<ArchetypeInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}