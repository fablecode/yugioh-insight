using carddata.core.Models;
using carddata.core.Processor;
using carddata.domain.Services.Messaging;
using carddata.domain.WebPages.Cards;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace carddata.domain.Processor
{
    public class ArticleDataFlow : IArticleDataFlow
    {
        private readonly ICardWebPage _cardWebPage;
        private readonly IYugiohCardQueue _yugiohCardQueue;

        public ArticleDataFlow(ICardWebPage cardWebPage, IYugiohCardQueue yugiohCardQueue)
        {
            _cardWebPage = cardWebPage;
            _yugiohCardQueue = yugiohCardQueue;
        }


        public async Task<ArticleCompletion> ProcessDataFlow(Article article)
        {
            var articleProcessed = _cardWebPage.GetYugiohCard(article);
            var yugiohCardCompletion = await _yugiohCardQueue.Publish(articleProcessed);

            if (yugiohCardCompletion.IsSuccessful)
            {
                return new ArticleCompletion { IsSuccessful = true, Message = yugiohCardCompletion.Article };
            }

            var exceptionMessage = $"Card Article with id '{yugiohCardCompletion.Article.Id}' and correlation id {yugiohCardCompletion.Article.CorrelationId} not processed.";

            throw new ArticleCompletionException(exceptionMessage);
        }

        #region private helper

        private static KeyValuePair<Guid, Article> TagInputData(Article data)
        {
            return new KeyValuePair<Guid, Article>(data.CorrelationId, data);
        }

        private static KeyValuePair<Guid, TaskCompletionSource<ArticleCompletion>> CreateJob(KeyValuePair<Guid, Article> taggedData)
        {
            var id = taggedData.Key;
            var jobCompletionSource = new TaskCompletionSource<ArticleCompletion>();
            return new KeyValuePair<Guid, TaskCompletionSource<ArticleCompletion>>(id, jobCompletionSource);
        }

        #endregion
    }

    public class ArticleCompletionException : Exception
    {
        public ArticleCompletionException(string exceptionMessage)
            : base (exceptionMessage)
        {
        }
    }
}