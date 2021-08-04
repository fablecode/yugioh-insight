using System.Collections.Generic;
using System.Linq;

namespace archetypedata.application.MessageConsumers.ArchetypeCardInformation
{
    public class ArchetypeCardInformationConsumerResult
    {
        public bool IsSuccessful => !Errors.Any();

        public List<string> Errors { get; set; } = new List<string>();
    }
}