using System;

namespace cardprocessor.application.MessageConsumers.CardData
{
    public class CardDataConsumerResult
    {
        public bool IsSuccessful { get; set; }
        public string Errors { get; set; }
        public Exception Exception { get; set; }
    }
}