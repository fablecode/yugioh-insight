using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;

namespace cardsectionprocessor.core.Services
{
    public interface ICardTriviaService
    {
        Task DeleteByCardId(long id);
        Task Update(List<TriviaSection> tips);
    }
}