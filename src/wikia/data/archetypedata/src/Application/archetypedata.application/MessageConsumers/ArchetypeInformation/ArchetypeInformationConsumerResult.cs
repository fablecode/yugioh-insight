using System.Collections.Generic;
using System.Linq;

namespace archetypedata.application.MessageConsumers.ArchetypeInformation
{
    public class ArchetypeInformationConsumerResult
    {
        public bool IsSuccessful => !Errors.Any();

        public string Message { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}