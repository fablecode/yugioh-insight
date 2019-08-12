using System;
using System.Collections.Generic;

namespace cardsectionprocessor.core.Models.Db
{
    public class Archetype
    {
        public Archetype()
        {
            ArchetypeCard = new HashSet<ArchetypeCard>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public ICollection<ArchetypeCard> ArchetypeCard { get; set; }
    }
}