using System;

namespace archetypeprocessor.core.Models
{
    public class ArchetypeMessage
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ProfileUrl { get; set; }
        public DateTime Revision { get; set; }
    }
}