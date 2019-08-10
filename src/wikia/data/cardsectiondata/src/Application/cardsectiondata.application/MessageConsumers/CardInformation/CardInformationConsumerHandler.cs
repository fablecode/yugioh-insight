using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cardsectiondata.application.MessageConsumers.CardInformation
{
    public class CardInformationConsumerHandler : IRequestHandler<CardInformationConsumer, CardInformationConsumerResult>
    {
        private readonly IArticleProcessor _articleProcessor;
        private readonly IValidator<CardInformationConsumer> _validator;
        private readonly ILogger<CardInformationConsumerHandler> _logger;

        public CardInformationConsumerHandler(IArticleProcessor articleProcessor, IValidator<CardInformationConsumer> validator, ILogger<CardInformationConsumerHandler> logger)
        {
            _articleProcessor = articleProcessor;
            _validator = validator;
            _logger = logger;
        }
        public async Task<CardInformationConsumerResult> Handle(CardInformationConsumer request, CancellationToken cancellationToken)
        {
            var cardInformationResult = new CardInformationConsumerResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var article = JsonConvert.DeserializeObject<Article>(request.Message);

                _logger.LogInformation("Processing category {@Category} '{@Title}'", request.Category, article.Title);
                var results = await _articleProcessor.Process(request.Category, article);
                _logger.LogInformation("Finished processing category {@Category} '{@Title}'", request.Category, article.Title);

                if (!results.IsSuccessful)
                {
                    cardInformationResult.Errors.AddRange(results.Errors);
                }
            }
            else
            {
                cardInformationResult.Errors = validationResults.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return cardInformationResult;
        }
    }
}