using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using archetypedata.core.Models;
using archetypedata.core.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace archetypedata.application.MessageConsumers.ArchetypeInformation
{
    public class ArchetypeInformationConsumerHandler : IRequestHandler<ArchetypeInformationConsumer, ArchetypeInformationConsumerResult>
    {
        private readonly IArchetypeProcessor _archetypeProcessor;
        private readonly IValidator<ArchetypeInformationConsumer> _validator;
        private readonly ILogger<ArchetypeInformationConsumerHandler> _logger;

        public ArchetypeInformationConsumerHandler(IArchetypeProcessor archetypeProcessor, IValidator<ArchetypeInformationConsumer> validator, ILogger<ArchetypeInformationConsumerHandler> logger)
        {
            _archetypeProcessor = archetypeProcessor;
            _validator = validator;
            _logger = logger;
        }
        public async Task<ArchetypeInformationConsumerResult> Handle(ArchetypeInformationConsumer request, CancellationToken cancellationToken)
        {
            var archetypeInformationConsumerResult = new ArchetypeInformationConsumerResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var archetypeArticle = JsonConvert.DeserializeObject<Article>(request.Message);

                _logger.LogInformation("Processing archetype '{@Title}'", archetypeArticle.Title);
                var results = await _archetypeProcessor.Process(archetypeArticle);
                _logger.LogInformation("Finished processing archetype '{@Title}'", archetypeArticle.Title);

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