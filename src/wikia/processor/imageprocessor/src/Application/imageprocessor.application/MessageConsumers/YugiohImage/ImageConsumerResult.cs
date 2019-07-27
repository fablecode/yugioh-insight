using System;

namespace imageprocessor.application.MessageConsumers.YugiohImage
{
    public class ImageConsumerResult
    {
        public bool IsSuccessful { get; set; }
        public string Errors { get; set; }
        public Exception Exception { get; set; }
    }
}