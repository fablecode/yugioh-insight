using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using article.core.Models;
using article.domain.ArticleList.Processor;
using Microsoft.Extensions.Logging;
using wikia.Models.Article.AlphabeticalList;

namespace article.application.Decorators.Loggers
{
    public class ArticleProcessorLoggerDecorator : IArticleProcessor
    {
        private readonly IArticleProcessor _articleProcessor;
        private readonly ILogger<ArticleProcessor> _logger;

        public ArticleProcessorLoggerDecorator(IArticleProcessor articleProcessor, ILogger<ArticleProcessor> logger)
        {
            _articleProcessor = articleProcessor;
            _logger = logger;
        }

        public async Task<ArticleTaskResult> Process(string category, UnexpandedArticle article)
        {
            try
            {
                _logger.LogInformation("Processing article '{@Title}', category '{@Category}'", article.Title, category);

                var articleResult = await _articleProcessor.Process(category, article);

                _logger.LogInformation("Finished processing article '{@Title}', category '{@Category}'", article.Title, category);

                return articleResult;
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error processing article '{@Title}', category '{@Category}'. Exception: {@Exception}", article.Title, category, ex);
                throw;
            }
        }
    }
}