﻿using System;
using System.Threading.Tasks;
using articledata.core.ArticleList.Processor;
using articledata.core.Exceptions;
using articledata.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.ArticleList.Processor
{
    public class ArticleBatchProcessor : IArticleBatchProcessor
    {
        private readonly IArticleProcessor _articleProcessor;

        public ArticleBatchProcessor(IArticleProcessor articleProcessor)
        {
            _articleProcessor = articleProcessor;
        }
        public async Task<ArticleBatchTaskResult> Process(string category, UnexpandedArticle[] articles)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException(nameof(category));

            if (articles == null)
                throw new ArgumentException(nameof(articles));

            var response = new ArticleBatchTaskResult();

            foreach (var article in articles)
            {
                try
                {
                    var result = await _articleProcessor.Process(category, article);

                    if (result.IsSuccessfullyProcessed)
                        response.Processed += 1;
                }
                catch (Exception ex)
                {
                    //_logger.Error("{1} | ' {0} '", article.Title, category);
                    //_logger.Error(ex);
                    response.Failed.Add(new ArticleException { Article = article, Exception = ex });
                }
            }

            return response;
        }
    }
}