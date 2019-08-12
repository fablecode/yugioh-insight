using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardsectionprocessor.core.Constants;
using cardsectionprocessor.core.Models;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.core.Service;
using cardsectionprocessor.core.Strategy;

namespace cardsectionprocessor.domain.Strategy
{
    public class CardRulingsProcessorStrategy : ICardSectionProcessorStrategy
    {
        private readonly ICardService _cardService;
        private readonly ICardRulingService _cardRulingService;

        public CardRulingsProcessorStrategy(ICardService cardService, ICardRulingService cardRulingService)
        {
            _cardService = cardService;
            _cardRulingService = cardRulingService;
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
                await _cardRulingService.DeleteByCardId(card.Id);

                var rulingSections = new List<RulingSection>();

                foreach (var cardSection in cardSectionData.CardSections)
                {
                    var rulingSection = new RulingSection
                    {
                        CardId = card.Id,
                        Name = cardSection.Name,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow
                    };

                    foreach (var ruling in cardSection.ContentList)
                    {
                        rulingSection.Ruling.Add(new Ruling
                        {
                            RulingSection = rulingSection,
                            Text = ruling,
                            Created = DateTime.UtcNow,
                            Updated = DateTime.UtcNow
                        });
                    }

                    rulingSections.Add(rulingSection);
                }

                if (rulingSections.Any())
                {
                    await _cardRulingService.Update(rulingSections);
                }
            }
            else
            {
                cardSectionDataTaskResult.Errors.Add($"Card Rulings: card '{cardSectionData.Name}' not found.");
            }


            return cardSectionDataTaskResult;
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardRulings;
        }
    }
}