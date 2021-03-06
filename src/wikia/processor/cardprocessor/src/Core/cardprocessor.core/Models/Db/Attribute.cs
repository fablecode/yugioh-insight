﻿using System;
using System.Collections.Generic;

namespace cardprocessor.core.Models.Db
{
    public class Attribute
    {
        public Attribute()
        {
            CardAttribute = new HashSet<CardAttribute>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public ICollection<CardAttribute> CardAttribute { get; set; }
    }
}