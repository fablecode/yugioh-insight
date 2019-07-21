using System.Collections.Generic;
using System.Threading.Tasks;
using archetypeprocessor.core.Models.Db;

namespace archetypeprocessor.core.Services
{
    public interface IArchetypeCardsService
    {
        Task<IEnumerable<ArchetypeCard>> Update(long archetypeId, IEnumerable<string> cards);
    }
}