﻿using System;

namespace archetypeprocessor.core.Models.Db
{
    public class CardRuling
    {
        public long Id { get; set; }
        public long CardId { get; set; }
        public string Ruling { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Card Card { get; set; }
    }
}