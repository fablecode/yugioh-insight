﻿using System;

namespace banlistprocessor.core.Models.Db
{
    public class CardTip
    {
        public long Id { get; set; }
        public long CardId { get; set; }
        public string Tip { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Card Card { get; set; }
    }
}