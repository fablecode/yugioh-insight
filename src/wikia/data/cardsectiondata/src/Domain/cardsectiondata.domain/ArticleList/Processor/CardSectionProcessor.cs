using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;
using cardsectiondata.domain.Helpers;
using wikia.Api;

namespace cardsectiondata.domain.ArticleList.Processor
{
    public class CardSectionProcessor : ICardSectionProcessor
    {
        private readonly IWikiArticle _wikiArticle;

        public CardSectionProcessor(IWikiArticle wikiArticle)
        {
            _wikiArticle = wikiArticle;
        }

        public async Task<CardSectionMessage> ProcessItem(Article article)
        {
            var cardSections = new List<CardSection>();

            var contentResult = await _wikiArticle.Simple(article.Id);

            foreach (var section in contentResult.Sections)
            {
                if (section.Title.Equals("References", StringComparison.OrdinalIgnoreCase))
                    continue;

                var rulingSection = new CardSection
                {
                    Name = section.Title,
                    ContentList = SectionHelper.GetSectionContentList(section)
                };

                cardSections.Add(rulingSection);
            }

            var cardSectionMessage = new CardSectionMessage { Name = article.Title, CardSections = cardSections };

            return cardSectionMessage;
        }
    }
}