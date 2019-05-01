using System.Threading.Tasks;
using cardprocessor.core.Enums;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;

namespace cardprocessor.core.Strategies
{
    public interface ICardTypeStrategy
    {
        Task<Card> Add(CardModel cardModel);
        Task<Card> Update(CardModel cardModel);

        bool Handles(YugiohCardType yugiohCardType);
    }
}