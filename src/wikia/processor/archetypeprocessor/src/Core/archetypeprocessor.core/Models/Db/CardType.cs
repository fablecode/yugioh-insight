﻿namespace archetypeprocessor.core.Models.Db
{
    public class CardType
    {
        public long TypeId { get; set; }
        public long CardId { get; set; }

        public Card Card { get; set; }
        public Type Type { get; set; }
    }
}