﻿using System;
using System.Collections.Generic;

namespace cardprocessor.core.Models.Db
{
    public class SubCategory
    {
        public SubCategory()
        {
            CardSubCategory = new HashSet<CardSubCategory>();
        }

        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Category Category { get; set; }
        public ICollection<CardSubCategory> CardSubCategory { get; set; }
    }
}