using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models;
using cardsectionprocessor.core.Processor;
using cardsectionprocessor.core.Strategy;

namespace cardsectionprocessor.domain.Processor
{
    public class CardSectionProcessor : ICardSectionProcessor
    {
        private readonly IEnumerable<ICardSectionProcessorStrategy> _cardSectionProcessorStrategies;

        public CardSectionProcessor(IEnumerable<ICardSectionProcessorStrategy> cardSectionProcessorStrategies)
        {
            _cardSectionProcessorStrategies = cardSectionProcessorStrategies;
        }
        public async Task<CardSectionDataTaskResult<CardSectionMessage>> Process(string category, CardSectionMessage cardSectionData)
        {
            var handler = _cardSectionProcessorStrategies.Single(h => h.Handles(category));

            var cardSectionDataTaskResult = await handler.Process(cardSectionData);

            return cardSectionDataTaskResult;
        }
    }
}