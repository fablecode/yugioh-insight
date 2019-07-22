using MediatR;

namespace archetypeprocessor.application.MessageConsumers.ArchetypeInformation
{
    public class ArchetypeInformationConsumer : IRequest<ArchetypeInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}