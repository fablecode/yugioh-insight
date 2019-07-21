using archetypeprocessor.core.Models.Db;
using archetypeprocessor.core.Services;
using archetypeprocessor.domain.Repository;
using System.Threading.Tasks;

namespace archetypeprocessor.domain.Services
{
    public class ArchetypeService : IArchetypeService
    {
        private readonly IArchetypeRepository _archetypeRepository;

        public ArchetypeService(IArchetypeRepository archetypeRepository)
        {
            _archetypeRepository = archetypeRepository;
        }

        public Task<Archetype> ArchetypeById(long id)
        {
            return _archetypeRepository.ArchetypeById(id);
        }

        public Task<Archetype> Add(Archetype archetype)
        {
            return _archetypeRepository.Add(archetype);
        }

        public Task<Archetype> Update(Archetype archetype)
        {
            return _archetypeRepository.Update(archetype);
        }
    }
}