using cardsectiondata.core.Constants;
using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;
using cardsectiondata.domain.Helpers;
using cardsectiondata.domain.WebPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wikia.Api;

namespace cardsectiondata.domain.ArticleList.Item
{
    public class CardTipItemProcessor : IArticleItemProcessor
    {
        private readonly IWikiArticle _wikiArticle;
        private readonly ITipRelatedWebPage _tipRelatedWebPage;
        private readonly ITipQueue _queue;

        public CardTipItemProcessor
        (
            IWikiArticle wikiArticle,
            ITipRelatedWebPage tipRelatedWebPage,
            ITipQueue queue
        )
        {
            _wikiArticle = wikiArticle;
            _tipRelatedWebPage = tipRelatedWebPage;
            _queue = queue;
        }

        public async Task<ArticleTaskResult> ProcessItem(Article article)
        {
            var response = new ArticleTaskResult { Article = article };

            var tipSections = new List<CardSection>();

            var articleCardTips = await _wikiArticle.Simple(article.Id);

            foreach (var cardTipSection in articleCardTips.Sections)
            {
                var tipSection = new CardSection
                {
                    Name = cardTipSection.Title,
                    ContentList = SectionHelper.GetSectionContentList(cardTipSection)
                };


                if (cardTipSection.Title.Equals("List", StringComparison.OrdinalIgnoreCase) ||
                    cardTipSection.Title.Equals("Lists", StringComparison.OrdinalIgnoreCase))
                {
                    tipSection.Name = tipSection.ContentList.First();
                    tipSection.ContentList.Clear();
                    _tipRelatedWebPage.GetTipRelatedCards(tipSection, article);
                }

                tipSections.Add(tipSection);
            }

            var cardSectionMessage = new CardSectionMessage { Name = article.Title, CardSections = tipSections };

            await _queue.Publish(cardSectionMessage);

            return response;
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardTips;
        }
    }

    public interface ITipQueue
    {
        Task Publish(CardSectionMessage message);
    }

    public class CardRulingItemProcessor : IArticleItemProcessor
    {
        public Task<ArticleTaskResult> ProcessItem(Article article)
        {
            throw new NotImplementedException();
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardRulings;
        }
    }

    public class CardTriviaItemProcessor : IArticleItemProcessor
    {
        public Task<ArticleTaskResult> ProcessItem(Article article)
        {
            throw new NotImplementedException();
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardTrivia;
        }
    }
}