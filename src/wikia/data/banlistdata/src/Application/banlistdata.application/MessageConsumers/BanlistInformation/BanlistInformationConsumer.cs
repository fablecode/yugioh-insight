using MediatR;

namespace banlistdata.application.MessageConsumers.BanlistInformation
{
    public class BanlistInformationConsumer : IRequest<BanlistInformationConsumerResult>
    {
        public string Message { get; set; }
    }
}