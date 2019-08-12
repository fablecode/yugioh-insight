using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardsectionprocessor.core.Constants;
using cardsectionprocessor.core.Models;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.core.Services;
using cardsectionprocessor.core.Strategy;

namespace cardsectionprocessor.domain.Strategy
{
    public class CardTriviaProcessorStrategy : ICardSectionProcessorStrategy
    {
        private readonly ICardService _cardService;
        private readonly ICardTriviaService _cardTriviaService;

        public CardTriviaProcessorStrategy(ICardService cardService, ICardTriviaService cardTriviaService)
        {
            _cardService = cardService;
            _cardTriviaService = cardTriviaService;
        }

        public async Task<CardSectionDataTaskResult<CardSectionMessage>> Process(CardSectionMessage cardSectionData)
        {
            var cardSectionDataTaskResult = new CardSectionDataTaskResult<CardSectionMessage>
            {
                CardSectionData = cardSectionData
            };

            var card = await _cardService.CardByName(cardSectionData.Name);

            if (card != null)
            {
                await _cardTriviaService.DeleteByCardId(card.Id);

                var triviaSections = new List<TriviaSection>();

                foreach (var cardSection in cardSectionData.CardSections)
                {
                    var triviaSection = new TriviaSection
                    {
                        CardId = card.Id,
                        Name = cardSection.Name,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow
                    };

                    foreach (var trivia in cardSection.ContentList)
                    {
                        triviaSection.Trivia.Add(new Trivia
                        {
                            TriviaSection = triviaSection,
                            Text = trivia,
                            Created = DateTime.UtcNow,
                            Updated = DateTime.UtcNow
                        });
                    }

                    triviaSections.Add(triviaSection);
                }

                if (triviaSections.Any())
                {
                    await _cardTriviaService.Update(triviaSections);
                }
            }
            else
            {
                cardSectionDataTaskResult.Errors.Add($"Card Trivia: card '{cardSectionData.Name}' not found.");
            }


            return cardSectionDataTaskResult;
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardTrivia;
        }
    }
}