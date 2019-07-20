using System;
using System.Threading.Tasks;
using archetypedata.core.Models;
using archetypedata.core.Processor;
using archetypedata.domain.Helpers;
using archetypedata.domain.Services.Messaging;
using archetypedata.domain.WebPages;

namespace archetypedata.domain.Processor
{
    public class ArchetypeCardProcessor : IArchetypeCardProcessor
    {
        private readonly IArchetypeWebPage _archetypeWebPage;
        private readonly IQueue<ArchetypeCard> _queue;

        public ArchetypeCardProcessor(IArchetypeWebPage archetypeWebPage, IQueue<ArchetypeCard> queue)
        {
            _archetypeWebPage = archetypeWebPage;
            _queue = queue;
        }

        public async Task<ArticleTaskResult> Process(Article article)
        {
            var articleTaskResult = new ArticleTaskResult { Article = article };

            var archetypeName = StringHelpers.ArchetypeNameFromListTitle(article.Title);

            var archetypeCards = new ArchetypeCard
            {
                ArchetypeName = archetypeName,
                Cards = _archetypeWebPage.Cards(new Uri(article.Url))
            };

            await _queue.Publish(archetypeCards);

            return articleTaskResult;
        }
    }
}