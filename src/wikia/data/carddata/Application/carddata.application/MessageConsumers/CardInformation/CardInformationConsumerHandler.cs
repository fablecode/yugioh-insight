using System.Threading;
using System.Threading.Tasks;
using carddata.core.Models;
using carddata.core.Processor;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace carddata.application.MessageConsumers.CardInformation
{
    public class CardInformationConsumerHandler : IRequestHandler<CardInformationConsumer, CardInformationConsumerResult>
    {
        private readonly IArticleProcessor _articleProcessor;
        private readonly IValidator<CardInformationConsumer> _validator;

        public CardInformationConsumerHandler(IArticleProcessor articleProcessor, IValidator<CardInformationConsumer> validator)
        {
            _articleProcessor = articleProcessor;
            _validator = validator;
        }
        public async Task<CardInformationConsumerResult> Handle(CardInformationConsumer request, CancellationToken cancellationToken)
        {
            var cardInformationTaskResult = new CardInformationConsumerResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var cardArticle = JsonConvert.DeserializeObject<Article>(request.Message);

                var results = await _articleProcessor.Process(cardArticle);

                cardInformationTaskResult.ArticleConsumerResult = results;
            }

            return cardInformationTaskResult;
        }
    }
}