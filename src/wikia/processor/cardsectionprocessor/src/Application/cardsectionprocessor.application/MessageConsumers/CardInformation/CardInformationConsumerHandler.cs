﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models;
using cardsectionprocessor.core.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace cardsectionprocessor.application.MessageConsumers.CardInformation
{
    public class CardInformationConsumerHandler : IRequestHandler<CardInformationConsumer, CardInformationConsumerResult>
    {
        private readonly ICardSectionProcessor _cardSectionProcessor;
        private readonly IValidator<CardInformationConsumer> _validator;
        private readonly ILogger<CardInformationConsumerHandler> _logger;

        public CardInformationConsumerHandler(ICardSectionProcessor cardSectionProcessor, IValidator<CardInformationConsumer> validator, ILogger<CardInformationConsumerHandler> logger)
        {
            _cardSectionProcessor = cardSectionProcessor;
            _validator = validator;
            _logger = logger;
        }
        public async Task<CardInformationConsumerResult> Handle(CardInformationConsumer request, CancellationToken cancellationToken)
        {
            var cardInformationResult = new CardInformationConsumerResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var article = JsonConvert.DeserializeObject<CardSectionMessage>(request.Message);

                _logger.LogInformation("Processing category {@Category} '{@Title}'", request.Category, article.Name);
                var results = await _cardSectionProcessor.Process(request.Category, article);
                _logger.LogInformation("Finished processing category {@Category} '{@Title}'", request.Category, article.Name);

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