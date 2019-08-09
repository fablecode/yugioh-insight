using cardsectiondata.core.Constants;
using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;
using cardsectiondata.domain.Services.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardsectiondata.domain.ArticleList.Item
{
    public class CardRulingItemProcessor : IArticleItemProcessor
    {
        private readonly ICardSectionProcessor _cardSectionProcessor;
        private readonly IEnumerable<IQueue> _queues;

        public CardRulingItemProcessor(ICardSectionProcessor cardSectionProcessor, IEnumerable<IQueue> queues)
        {
            _cardSectionProcessor = cardSectionProcessor;
            _queues = queues;
        }

        public async Task<ArticleTaskResult> ProcessItem(Article article)
        {
            var response = new ArticleTaskResult { Article = article };

            var cardSectionMessage = await _cardSectionProcessor.ProcessItem(article);

            await _queues.Single(q => q.Handles(ArticleCategory.CardRulings)).Publish(cardSectionMessage);

            return response;
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.CardRulings;
        }
    }
}