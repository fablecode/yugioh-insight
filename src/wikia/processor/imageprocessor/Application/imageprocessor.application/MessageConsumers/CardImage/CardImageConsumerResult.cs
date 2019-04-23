using System;

namespace imageprocessor.application.MessageConsumers.CardImage
{
    public class CardImageConsumerResult
    {
        public bool IsSuccessful { get; set; }
        public string Errors { get; set; }
        public Exception Exception { get; set; }
    }
}