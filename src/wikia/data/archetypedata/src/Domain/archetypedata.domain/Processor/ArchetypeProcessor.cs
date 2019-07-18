﻿using archetypedata.core.Models;
using archetypedata.domain.Helpers;
using archetypedata.domain.Services.Messaging;
using archetypedata.domain.WebPages;
using System;
using System.Threading.Tasks;
using archetypedata.core.Processor;

namespace archetypedata.domain.Processor
{
    public class ArchetypeProcessor : IArchetypeProcessor
    {
        private readonly IArchetypeWebPage _archetypeWebPage;
        private readonly IQueue<Archetype> _queue;

        public ArchetypeProcessor(IArchetypeWebPage archetypeWebPage, IQueue<Archetype> queue)
        {
            _archetypeWebPage = archetypeWebPage;
            _queue = queue;
        }
        public async Task<ArticleTaskResult> Process(Article article)
        {
            var articleTaskResult = new ArticleTaskResult { Article = article };

            if (!article.Title.Equals("Archetype", StringComparison.OrdinalIgnoreCase))
            {
                var thumbNailUrl = await _archetypeWebPage.ArchetypeThumbnail(article.Id, article.Url);

                var archetype = new Archetype
                {
                    Id = article.Id,
                    Name = article.Title,
                    ImageUrl = ImageHelper.ExtractImageUrl(thumbNailUrl),
                    ProfileUrl = article.Url
                };

                await _queue.Publish(archetype);

                articleTaskResult.IsSuccessful = true;
            }

            return articleTaskResult;
        }
    }
}
