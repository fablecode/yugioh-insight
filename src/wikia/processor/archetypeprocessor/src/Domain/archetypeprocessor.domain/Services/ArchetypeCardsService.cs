using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archetypeprocessor.core.Models.Db;
using archetypeprocessor.core.Services;
using archetypeprocessor.domain.Repository;

namespace archetypeprocessor.domain.Services
{
    public sealed class ArchetypeCardsService : IArchetypeCardsService
    {
        private readonly IArchetypeCardsRepository _archetypeCardsRepository;

        public ArchetypeCardsService(IArchetypeCardsRepository archetypeCardsRepository)
        {
            _archetypeCardsRepository = archetypeCardsRepository;
        }
        public Task<IEnumerable<ArchetypeCard>> Update(long archetypeId, IEnumerable<string> cards)
        {
            var archetypeCards = cards.Distinct().ToList();

            return _archetypeCardsRepository.Update(archetypeId, archetypeCards);
        }
    }
}