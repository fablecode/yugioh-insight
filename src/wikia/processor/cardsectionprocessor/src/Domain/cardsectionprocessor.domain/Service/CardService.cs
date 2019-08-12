using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.core.Service;
using cardsectionprocessor.domain.Repository;

namespace cardsectionprocessor.domain.Service
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        public Task<Card> CardByName(string name)
        {
            return _cardRepository.CardByName(name);
        }
    }
}