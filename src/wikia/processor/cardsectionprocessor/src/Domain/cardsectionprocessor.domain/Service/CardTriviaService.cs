using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.core.Service;
using cardsectionprocessor.domain.Repository;

namespace cardsectionprocessor.domain.Service
{
    public class CardTriviaService : ICardTriviaService
    {
        private readonly ICardTriviaRepository _cardTriviaRepository;

        public CardTriviaService(ICardTriviaRepository cardTriviaRepository)
        {
            _cardTriviaRepository = cardTriviaRepository;
        }

        public Task DeleteByCardId(long id)
        {
            return _cardTriviaRepository.DeleteByCardId(id);
        }

        public Task Update(List<TriviaSection> triviaSections)
        {
            return _cardTriviaRepository.Update(triviaSections);
        }
    }
}