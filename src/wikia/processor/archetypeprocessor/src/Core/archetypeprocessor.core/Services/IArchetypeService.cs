using System.Threading.Tasks;
using archetypeprocessor.core.Models.Db;

namespace archetypeprocessor.core.Services
{
    public interface IArchetypeService
    {
        Task<Archetype> ArchetypeByName(string name);
        Task<Archetype> ArchetypeById(long id);
        Task<Archetype> Add(Archetype archetype);
        Task<Archetype> Update(Archetype archetype);
    }
}