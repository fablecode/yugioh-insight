using System.Threading;
using System.Threading.Tasks;
using archetypedata.core.Models;
using archetypedata.core.Processor;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace archetypedata.application.MessageConsumers.ArchetypeInformation
{
    public class ArchetypeInformationConsumerHandler : IRequestHandler<ArchetypeInformationConsumer, ArchetypeInformationConsumerResult>
    {
        private readonly IArchetypeProcessor _archetypeProcessor;
        private readonly IValidator<ArchetypeInformationConsumer> _validator;

        public ArchetypeInformationConsumerHandler(IArchetypeProcessor archetypeProcessor, IValidator<ArchetypeInformationConsumer> validator)
        {
            _archetypeProcessor = archetypeProcessor;
            _validator = validator;
        }
        public async Task<ArchetypeInformationConsumerResult> Handle(ArchetypeInformationConsumer request, CancellationToken cancellationToken)
        {
            var archetypeInformationConsumerResult = new ArchetypeInformationConsumerResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var cardArticle = JsonConvert.DeserializeObject<Article>(request.Message);

                var results = await _archetypeProcessor.Process(cardArticle);

                if (!results.IsSuccessful)
                {
                    archetypeInformationConsumerResult.Errors.AddRange(results.Errors);
                }
            }

            return archetypeInformationConsumerResult;
        }
    }
}