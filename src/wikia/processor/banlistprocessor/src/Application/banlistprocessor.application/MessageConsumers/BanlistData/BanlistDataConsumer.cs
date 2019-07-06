using MediatR;

namespace banlistprocessor.application.MessageConsumers.BanlistData
{
    public class BanlistDataConsumer : IRequest<BanlistDataConsumerResult>
    {
        public string Message { get; set; }
    }
}