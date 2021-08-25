using System.Collections.Generic;

namespace Cards.Domain.Models.Monsters
{
    public sealed class EffectMonster
    {
        public string Name { get; set; }
        public string Attribute { get; set; }
        public int Level { get; set; }
        public IReadOnlySet<string> Types { get; } = new HashSet<string>();
        public string Effect { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
    }
}