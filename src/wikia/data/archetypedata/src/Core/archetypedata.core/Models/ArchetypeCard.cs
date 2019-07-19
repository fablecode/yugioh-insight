using System.Collections.Generic;

namespace archetypedata.core.Models
{
    public class ArchetypeCard
    {
        public string ArchetypeName { get; set; }
        public IEnumerable<string> Cards { get; set; }
    }
}