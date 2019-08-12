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
    public class CardTipsProcessorStrategy : ICardSectionProcessorStrategy
    {
        private readonly ICardService _cardService;
        private readonly ICardTipService _cardTipService;

        public CardTipsProcessorStrategy(ICardService cardService, ICardTipService cardTipService)
        {
            _cardService = cardService;
            _cardTipService = cardTipService;
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
                await _cardTipService.DeleteByCardId(card.Id);

                var newTipSectionList = new List<TipSection>();

                foreach (var cardSection in cardSectionData.CardSections)
                {
                    var newTipSection = new TipSection
                    {
                        CardId = card.Id,
                        Name = cardSection.Name,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow
                    };

                    foreach (var tip in cardSection.ContentList)
                    {
                        newTipSection.Tip.Add(new Tip
                        {
                            TipSection = newTipSection,
                            Text = tip,
                            Created = DateTime.UtcNow,
                            Updated = DateTime.UtcNow
                        });
                    }

                    newTipSectionList.Add(newTipSection);
                }

                if (newTipSectionList.Any())
                {
                    await _cardTipService.Update(newTipSectionList);
                }
            }
            else
            {
                cardSectionDataTaskResult.Errors.Add($"Card Tips: card '{cardSectionData.Name}' not found.");
            }


            return cardSectionDataTaskResult;
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardTips;
        }
    }
}