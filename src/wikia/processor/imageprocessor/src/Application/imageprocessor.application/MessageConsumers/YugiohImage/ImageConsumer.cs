using MediatR;

namespace imageprocessor.application.MessageConsumers.YugiohImage
{
    public class ImageConsumer : IRequest<ImageConsumerResult>
    {
        public string Message { get; set; }
    }
}