﻿using System.Collections.Generic;

namespace cardsectiondata.core.Models
{
    public class CardSection
    {
        public string Name { get; set; }
        public List<string> ContentList { get; set; } = new List<string>();
    }
}