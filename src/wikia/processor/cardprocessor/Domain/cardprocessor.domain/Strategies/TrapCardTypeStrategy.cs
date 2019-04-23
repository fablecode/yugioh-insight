using System.Threading.Tasks;
using cardprocessor.core.Enums;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Strategies;
using cardprocessor.domain.Mappers;
using cardprocessor.domain.Repository;

namespace cardprocessor.domain.Strategies
{
    public class TrapCardTypeStrategy : ICardTypeStrategy
    {
        private readonly ICardRepository _cardRepository;

        public TrapCardTypeStrategy(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public async Task<Card> Add(CardModel cardModel)
        {
            var newTrapCard = CardMapper.MapToSpellOrTrapCard(cardModel);

            return await _cardRepository.Add(newTrapCard);
        }
        public async Task<Card> Update(CardModel cardModel)
        {
            var cardToUpdate = await _cardRepository.CardById(cardModel.Id);

            if (cardToUpdate != null)
            {
                CardMapper.UpdateTrapCardWith(cardToUpdate, cardModel);
                return await _cardRepository.Update(cardToUpdate);
            }

            return null;
        }
        public bool Handles(YugiohCardType yugiohCardType)
        {
            return yugiohCardType == YugiohCardType.Trap;
        }
    }
}