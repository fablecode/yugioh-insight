﻿using System;
using System.Collections.Generic;

namespace archetypeprocessor.core.Models.Db
{
    public class LinkArrow
    {
        public LinkArrow()
        {
            CardLinkArrow = new HashSet<CardLinkArrow>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public ICollection<CardLinkArrow> CardLinkArrow { get; set; }
    }
}