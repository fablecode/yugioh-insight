using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using article.core.Constants;
using article.core.Enums;
using article.core.Models;
using article.domain.Banlist.DataSource;
using Microsoft.Extensions.Logging;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.Banlist.Processor
{
    public sealed class BanlistProcessor : IBanlistProcessor
    {
        private readonly IBanlistUrlDataSource _banlistUrlDataSource;
        private readonly IArticleProcessor _articleProcessor;
        private readonly ILogger<BanlistProcessor> _logger;
        
        public BanlistProcessor(IBanlistUrlDataSource banlistUrlDataSource, IArticleProcessor articleProcessor, ILogger<BanlistProcessor> logger)
        {
            _banlistUrlDataSource = banlistUrlDataSource;
            _articleProcessor = articleProcessor;
            _logger = logger;
        }

        public async Task<ArticleBatchTaskResult> Process(BanlistType banlistType)
        {
            var response = new ArticleBatchTaskResult();

            const string baseBanlistUrl = "http://yugioh.fandom.com/wiki/July_1999_Lists";
            var banListArticleIds = _banlistUrlDataSource.GetBanlists(banlistType, baseBanlistUrl);

            foreach (var (year, banlistIds) in banListArticleIds)
            {
                _logger.LogInformation("{@BanlistType} banlists for the year: {@Year}", banlistType.ToString().ToUpper(), year);

                foreach (var articleId in banlistIds)
                {
                    _logger.LogInformation("{@BanlistType} banlist articleId: {@ArticleId}", banlistType.ToString().ToUpper(), articleId);

                    var articleResult = await _articleProcessor.Process(ArticleCategory.ForbiddenAndLimited, new UnexpandedArticle {Id = articleId});

                    if (articleResult.IsSuccessfullyProcessed)
                        response.Processed += 1;
                }
            }

            return response;
        }
    }
}