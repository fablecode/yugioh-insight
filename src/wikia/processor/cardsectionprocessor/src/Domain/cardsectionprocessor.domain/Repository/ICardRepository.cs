using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;

namespace cardsectionprocessor.domain.Repository
{
    public interface ICardRepository
    {
        Task<Card> CardByName(string name);
    }
}