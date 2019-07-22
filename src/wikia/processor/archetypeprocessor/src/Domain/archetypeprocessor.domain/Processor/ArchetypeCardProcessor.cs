using archetypeprocessor.core.Models;
using archetypeprocessor.core.Processor;
using archetypeprocessor.core.Services;
using System.Threading.Tasks;

namespace archetypeprocessor.domain.Processor
{
    public class ArchetypeCardProcessor : IArchetypeCardProcessor
    {
        private readonly IArchetypeService _archetypeService;
        private readonly IArchetypeCardsService _archetypeCardsService;

        public ArchetypeCardProcessor(IArchetypeService archetypeService, IArchetypeCardsService archetypeCardsService)
        {
            _archetypeService = archetypeService;
            _archetypeCardsService = archetypeCardsService;
        }

        public async Task<ArchetypeDataTaskResult<ArchetypeCardMessage>> Process(ArchetypeCardMessage archetypeData)
        {
            var articleDataTaskResult = new ArchetypeDataTaskResult<ArchetypeCardMessage>
            {
                ArchetypeData = archetypeData
            };

            var existingArchetype = await _archetypeService.ArchetypeByName(archetypeData.ArchetypeName);

            if (existingArchetype != null)
            {
                await _archetypeCardsService.Update(existingArchetype.Id, archetypeData.Cards);
            }

            return articleDataTaskResult;
        }
    }
}