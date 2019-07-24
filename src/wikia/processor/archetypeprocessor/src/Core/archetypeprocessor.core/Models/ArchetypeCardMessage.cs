using System.Collections.Generic;

namespace archetypeprocessor.core.Models
{
    public class ArchetypeCardMessage
    {
        public string ArchetypeName { get; set; }
        public IEnumerable<string> Cards { get; set; } = new List<string>();
    }
}