using System;
using System.Collections.Generic;

namespace cardsectionprocessor.core.Models.Db
{
    public class Type
    {
        public Type()
        {
            CardType = new HashSet<CardType>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public ICollection<CardType> CardType { get; set; }
    }
}