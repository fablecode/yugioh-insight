using carddata.core.Exceptions;
using carddata.core.Models;
using carddata.core.Processor;
using carddata.domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
            var response = new ArticleConsumerResult {Article = article};

            try
            {
                _logger.LogInformation($"{article.Title} processing... ");
                var result = await _articleDataFlow.ProcessDataFlow(article);

                if (result.IsSuccessful)
                {
                    _logger.LogInformation($"{article.Title} processed successfully. ");
                    response.IsSuccessfullyProcessed = true;
                }
                else
                {
                    _logger.LogInformation($"{article.Title} processing failed. ");
                }
            }
            catch (ArticleCompletionException ex )
            {
                _logger.LogError(" {ArticleTitle} error. Exception: {@Exception}, Article: {Article}", article.Title, ex, article);
               response.Failed = new ArticleException { Article = article, Exception = ex };
            }

            return response;
        }
    }
}