using System;
using System.Threading.Tasks;
using carddata.core.Exceptions;
using carddata.core.Models;
using carddata.core.Processor;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace carddata.domain.Processor
{
    public class ArticleProcessor : IArticleProcessor
    {
        private readonly IArticleDataFlow _articleDataFlow;
        private readonly ILogger<ArticleProcessed> _logger;

        public ArticleProcessor(IArticleDataFlow articleDataFlow, ILogger<ArticleProcessed> logger)
        {
            _articleDataFlow = articleDataFlow;
            _logger = logger;
        }

        public async Task<ArticleConsumerResult> Process(Article article)
        {
            var articleJson = JsonConvert.SerializeObject(article);

            var response = new ArticleConsumerResult();
            response.Article = articleJson;

            try
            {
                _logger.LogInformation($"{article.Title} Processing... ");
                var result = await _articleDataFlow.ProcessDataFlow(article);

                if (result.IsSuccessful)
                {
                    _logger.LogInformation($"{article.Title} Processed successfully. ");
                    response.IsSuccessfullyProcessed = true;
                }
                else
                {
                    _logger.LogInformation($"{article.Title} Processing failed. ");
                }
            }
            catch (Exception ex )
            {
               response.Failed = new ArticleException { Article = articleJson, Exception = ex };
            }

            return response;
        }
    }
}