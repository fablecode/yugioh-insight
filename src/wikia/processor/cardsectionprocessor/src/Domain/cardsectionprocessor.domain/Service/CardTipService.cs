using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.core.Service;
using cardsectionprocessor.domain.Repository;

namespace cardsectionprocessor.domain.Service
{
    public class CardTipService : ICardTipService
    {
        private readonly ICardTipRepository _cardTipRepository;

        public CardTipService(ICardTipRepository cardTipRepository)
        {
            _cardTipRepository = cardTipRepository;
        }

        public Task DeleteByCardId(long id)
        {
            return _cardTipRepository.DeleteByCardId(id);
        }

        public Task Update(List<TipSection> tips)
        {
            return _cardTipRepository.Update(tips);
        }
    }
}