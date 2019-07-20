using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using archetypedata.application.MessageConsumers.ArchetypeInformation;
using archetypedata.core.Models;
using archetypedata.core.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace archetypedata.application.MessageConsumers.ArchetypeCardInformation
{
    public class ArchetypeCardInformationConsumerHandler : IRequestHandler<ArchetypeCardInformationConsumer, ArchetypeCardInformationConsumerResult>
    {
        private readonly IArchetypeCardProcessor _archetypeCardProcessor;
        private readonly IValidator<ArchetypeCardInformationConsumer> _validator;
        private readonly ILogger<ArchetypeCardInformationConsumerHandler> _logger;

        public ArchetypeCardInformationConsumerHandler(IArchetypeCardProcessor archetypeCardProcessor, IValidator<ArchetypeCardInformationConsumer> validator, ILogger<ArchetypeCardInformationConsumerHandler> logger)
        {
            _archetypeCardProcessor = archetypeCardProcessor;
            _validator = validator;
            _logger = logger;
        }
        public async Task<ArchetypeCardInformationConsumerResult> Handle(ArchetypeCardInformationConsumer request, CancellationToken cancellationToken)
        {
            var archetypeInformationConsumerResult = new ArchetypeCardInformationConsumerResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var archetypeArticle = JsonConvert.DeserializeObject<Article>(request.Message);

                _logger.LogInformation("Processing archetype card '{@Title}'", archetypeArticle.Title);
                var results = await _archetypeCardProcessor.Process(archetypeArticle);
                _logger.LogInformation("Finished processing archetype card '{@Title}'", archetypeArticle.Title);

                if (!results.IsSuccessful)
                {
                    archetypeInformationConsumerResult.Errors.AddRange(results.Errors);
                }
            }
            else
            {
                archetypeInformationConsumerResult.Errors = validationResults.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return archetypeInformationConsumerResult;
        }
    }
}