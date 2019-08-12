using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;

namespace cardsectionprocessor.core.Services
{
    public interface ICardRulingService
    {
        Task DeleteByCardId(long id);
        Task Update(List<RulingSection> tips);
    }
}