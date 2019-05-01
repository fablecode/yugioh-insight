using System.Threading.Tasks;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;

namespace cardprocessor.core.Services
{
    public interface ICardService
    {
        Task<Card> Add(CardModel cardModel);
        Task<Card> Update(CardModel cardModel);
        Task<Card> CardById(long cardId);
        Task<Card> CardByName(string name);
        Task<bool> CardExists(long id);
    }
}