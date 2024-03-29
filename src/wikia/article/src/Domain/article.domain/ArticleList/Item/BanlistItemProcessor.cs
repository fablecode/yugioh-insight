﻿using article.core.ArticleList.Processor;
using article.core.Constants;
using article.core.Exceptions;
using article.core.Models;
using article.domain.Services.Messaging;
using System;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.ArticleList.Item
{
    public sealed class BanlistItemProcessor : IArticleItemProcessor
    {
        private readonly IBanlistArticleQueue _banlistArticleQueue;

        public BanlistItemProcessor(IBanlistArticleQueue banlistArticleQueue)
        {
            _banlistArticleQueue = banlistArticleQueue;
        }
        public async Task<ArticleTaskResult> ProcessItem(UnexpandedArticle item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var articleTaskResult = new ArticleTaskResult { Article = item };

            try
            {
                await _banlistArticleQueue.Publish(item);

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
            return category == ArticleCategory.ForbiddenAndLimited;
        }
    }
}