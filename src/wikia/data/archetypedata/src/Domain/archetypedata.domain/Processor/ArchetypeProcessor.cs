using archetypedata.core.Models;
using archetypedata.domain.Helpers;
using archetypedata.domain.Services.Messaging;
using archetypedata.domain.WebPages;
using System;
using System.Linq;
using System.Threading.Tasks;
using archetypedata.core.Processor;
using wikia.Api;

namespace archetypedata.domain.Processor
{
    public class ArchetypeProcessor : IArchetypeProcessor
    {
        private readonly IArchetypeWebPage _archetypeWebPage;
        private readonly IQueue<Archetype> _queue;
        private readonly IWikiArticle _wikiArticle;

        public ArchetypeProcessor(IArchetypeWebPage archetypeWebPage, IQueue<Archetype> queue, IWikiArticle wikiArticle)
        {
            _archetypeWebPage = archetypeWebPage;
            _queue = queue;
            _wikiArticle = wikiArticle;
        }
        public async Task<ArticleTaskResult> Process(Article article)
        {
            var articleTaskResult = new ArticleTaskResult { Article = article };

            if (!article.Title.Equals("Archetype", StringComparison.OrdinalIgnoreCase))
            {
                var articleDetailsList = await _wikiArticle.Details((int)article.Id);

                var articleDetails = articleDetailsList.Items.First();

                var epoch = articleDetails.Value.Revision.Timestamp;

                var thumbNailUrl = await _archetypeWebPage.ArchetypeThumbnail(article.Id, article.Url);

                var archetype = new Archetype
                {
                    Id = article.Id,
                    Name = article.Title,
                    Revision = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch),
                    ImageUrl = ImageHelper.ExtractImageUrl(thumbNailUrl),
                    ProfileUrl = article.Url
                };

                await _queue.Publish(archetype);
            }

            return articleTaskResult;
        }
    }
}
