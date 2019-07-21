using System.Collections.Generic;
using System.Threading.Tasks;
using archetypeprocessor.core.Models.Db;

namespace archetypeprocessor.domain.Repository
{
    public interface IArchetypeCardsRepository
    {
        Task<IEnumerable<ArchetypeCard>> Update(long archetypeId, IEnumerable<string> cards);
    }
}