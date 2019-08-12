using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.core.Service;
using cardsectionprocessor.domain.Repository;

namespace cardsectionprocessor.domain.Service
{
    public class CardRulingService : ICardRulingService
    {
        private readonly ICardRulingRepository _cardRulingRepository;

        public CardRulingService(ICardRulingRepository cardRulingRepository)
        {
            _cardRulingRepository = cardRulingRepository;
        }

        public Task DeleteByCardId(long id)
        {
            return _cardRulingRepository.DeleteByCardId(id);
        }

        public Task Update(List<RulingSection> tipSections)
        {
            return _cardRulingRepository.Update(tipSections);
        }
    }
}