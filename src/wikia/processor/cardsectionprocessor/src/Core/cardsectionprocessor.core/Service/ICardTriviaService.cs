using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;

namespace cardsectionprocessor.core.Service
{
    public interface ICardTriviaService
    {
        Task DeleteByCardId(long id);
        Task Update(List<TriviaSection> triviaSections);
    }
}