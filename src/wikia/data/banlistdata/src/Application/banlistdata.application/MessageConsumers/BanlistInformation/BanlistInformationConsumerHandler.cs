using System.Threading;
using System.Threading.Tasks;
using banlistdata.core.Models;
using banlistdata.core.Processor;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;


namespace banlistdata.application.MessageConsumers.BanlistInformation
{
    public class BanlistInformationConsumerHandler : IRequestHandler<BanlistInformationConsumer, BanlistInformationConsumerResult>
    {
        private readonly IArticleProcessor _articleProcessor;
        private readonly IValidator<BanlistInformationConsumer> _validator;

        public BanlistInformationConsumerHandler(IArticleProcessor articleProcessor, IValidator<BanlistInformationConsumer> validator)
        {
            _articleProcessor = articleProcessor;
            _validator = validator;
        }

        public async Task<BanlistInformationConsumerResult> Handle(BanlistInformationConsumer request, CancellationToken cancellationToken)
        {
            var response = new BanlistInformationConsumerResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var banlistArticle = JsonConvert.DeserializeObject<Article>(request.Message);

                var results = await _articleProcessor.Process(banlistArticle);

                if(!results.IsSuccessfullyProcessed)
                    response.Errors.Add(results.Failed.Exception.Message);

                response.Message = request.Message;
            }

            return response;
        }
    }
}