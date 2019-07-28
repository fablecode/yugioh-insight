using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using cardsectiondata.core.Constants;
using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace cardsectiondata.application.MessageConsumers.CardTipInformation
{
    public class CardTipInformationConsumerHandler : IRequestHandler<CardTipInformationConsumer, CardTipInformationConsumerResult>
    {
        private readonly IArticleProcessor _articleProcessor;
        private readonly IValidator<CardTipInformationConsumer> _validator;
        private readonly ILogger<CardTipInformationConsumerHandler> _logger;

        public CardTipInformationConsumerHandler(IArticleProcessor articleProcessor, IValidator<CardTipInformationConsumer> validator, ILogger<CardTipInformationConsumerHandler> logger)
        {
            _articleProcessor = articleProcessor;
            _validator = validator;
            _logger = logger;
        }
        public async Task<CardTipInformationConsumerResult> Handle(CardTipInformationConsumer request, CancellationToken cancellationToken)
        {
            var cardTipInformationConsumerResult = new CardTipInformationConsumerResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var cardTipArticle = JsonConvert.DeserializeObject<Article>(request.Message);

                _logger.LogInformation("Processing card tip '{@Title}'", cardTipArticle.Title);
                var results = await _articleProcessor.Process(ArticleCategory.CardTips, cardTipArticle);
                _logger.LogInformation("Finished processing card tip '{@Title}'", cardTipArticle.Title);

                if (!results.IsSuccessful)
                {
                    cardTipInformationConsumerResult.Errors.AddRange(results.Errors);
                }
            }
            else
            {
                cardTipInformationConsumerResult.Errors = validationResults.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return cardTipInformationConsumerResult;
        }
    }
}