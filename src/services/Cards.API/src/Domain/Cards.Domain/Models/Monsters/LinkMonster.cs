using System.Collections.Generic;

namespace Cards.Domain.Models.Monsters
{
    public sealed class LinkMonster
    {
        public string Name { get; set; }
        public string Attribute { get; set; }
        public int LinkRating { get; set; }
        public IReadOnlySet<string> LinkArrows { get; } = new HashSet<string>();
        public IReadOnlySet<string> Types { get; } = new HashSet<string>();
        public string Effect { get; set; }
        public int Attack { get; set; }
    }
}