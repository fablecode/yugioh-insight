using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;

namespace banlistprocessor.domain.Repository
{
    public interface ICardRepository
    {
        Task<Card> CardByName(string cardName);
    }
}