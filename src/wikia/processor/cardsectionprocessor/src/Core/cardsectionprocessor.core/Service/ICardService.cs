using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;

namespace cardsectionprocessor.core.Service
{
    public interface ICardService
    {
        Task<Card> CardByName(string name);
    }
}