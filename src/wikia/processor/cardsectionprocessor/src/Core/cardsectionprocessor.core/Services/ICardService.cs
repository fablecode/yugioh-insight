using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;

namespace cardsectionprocessor.core.Services
{
    public interface ICardService
    {
        Task<Card> CardByName(string name);
    }
}