using article.core.ArticleList.Processor;
using article.core.Constants;
using article.core.Exceptions;
using article.core.Models;
using article.domain.Services.Messaging;
using System;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.ArticleList.Item
{
    public class ArchetypeItemProcessor : IArticleItemProcessor
    {
        private readonly IArchetypeArticleQueue _archetypeArticleQueue;

        public ArchetypeItemProcessor(IArchetypeArticleQueue archetypeArticleQueue)
        {
            _archetypeArticleQueue = archetypeArticleQueue;
        }

        public async Task<ArticleTaskResult> ProcessItem(UnexpandedArticle item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var articleTaskResult = new ArticleTaskResult { Article = item };

            try
            {
                await _archetypeArticleQueue.Publish(item);

                articleTaskResult.IsSuccessfullyProcessed = true;
            }
            catch (Exception ex)
            {
                articleTaskResult.Failed = new ArticleException { Article = item, Exception = ex };
            }

            return articleTaskResult;
        }

        public bool Handles(string category)
        {
            return category == ArticleCategory.Archetype ||
                   category == ArticleCategory.CardsByArchetype ||
                   category == ArticleCategory.CardsByArchetypeSupport;
        }
    }
}