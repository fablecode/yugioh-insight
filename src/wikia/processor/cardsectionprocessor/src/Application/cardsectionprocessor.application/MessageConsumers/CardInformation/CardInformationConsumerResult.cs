﻿using System.Collections.Generic;
using System.Linq;

namespace cardsectionprocessor.application.MessageConsumers.CardInformation
{
    public class CardInformationConsumerResult
    {
        public bool IsSuccessful => !Errors.Any();

        public string Message { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}