using System;

namespace archetypedata.core.Models
{
    public class Archetype
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ProfileUrl { get; set; }
        public DateTime Revision { get; set; }
    }
}