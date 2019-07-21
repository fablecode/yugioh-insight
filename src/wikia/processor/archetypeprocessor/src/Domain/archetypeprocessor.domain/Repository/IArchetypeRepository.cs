using System.Threading.Tasks;
using archetypeprocessor.core.Models.Db;

namespace archetypeprocessor.domain.Repository
{
    public interface IArchetypeRepository
    {
        Task<Archetype> ArchetypeById(long id);
        Task<Archetype> Add(Archetype archetype);
        Task<Archetype> Update(Archetype archetype);
    }
}