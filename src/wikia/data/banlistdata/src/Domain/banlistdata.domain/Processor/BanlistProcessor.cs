using banlistdata.core.Models;
using banlistdata.core.Processor;
using banlistdata.domain.Helpers;
using banlistdata.domain.Services.Messaging;
using System.Linq;
using System.Threading.Tasks;
using wikia.Api;

namespace banlistdata.domain.Processor
{
    public class BanlistProcessor : IBanlistProcessor
    {
        private readonly IWikiArticle _wikiArticle;
        private readonly IBanlistDataQueue _banlistDataQueue;

        public BanlistProcessor(IWikiArticle wikiArticle, IBanlistDataQueue banlistDataQueue)
        {
            _wikiArticle = wikiArticle;
            _banlistDataQueue = banlistDataQueue;
        }

        public async Task<ArticleProcessed> Process(Article article)
        {
            var response = new ArticleProcessed { Article = article };

            var articleDetailsList = await _wikiArticle.Details((int) article.Id);

            var (_, expandedArticle) = articleDetailsList.Items.First();

            var banlistArticleSummary = BanlistHelpers.ExtractBanlistArticleDetails(expandedArticle.Id, expandedArticle.Abstract);

            if (banlistArticleSummary != null)
            {
                const char beginChar = '「';
                const char endChar = '」';

                var banlist = new YugiohBanlist
                {
                    ArticleId = banlistArticleSummary.ArticleId,
                    Title = expandedArticle.Title,
                    BanlistType = banlistArticleSummary.BanlistType,
                    StartDate = banlistArticleSummary.StartDate
                };

                var banlistContentResult = await _wikiArticle.Simple(banlistArticleSummary.ArticleId);

                foreach (var section in banlistContentResult.Sections)
                {
                    // skip references section
                    if (section.Title.ToLower() == "references")
                        continue;

                    // new section
                    var ybls = new YugiohBanlistSection
                    {
                        Title = StringHelpers.RemoveBetween(section.Title, beginChar, endChar).Trim(),
                        Content = ContentResultHelpers.GetSectionContentList(section).OrderBy(c => c).ToList()
                    };

                    // remove invalid characters
                    if (ybls.Content.Any())
                        ybls.Content = ybls.Content.Select(c => StringHelpers.RemoveBetween(c, beginChar, endChar)).ToList();

                    banlist.Sections.Add(ybls);
                }

                response.Banlist = banlist;

                var publishBanlistResult = await _banlistDataQueue.Publish(banlist);

                response.IsSuccessful = publishBanlistResult.IsSuccessful;
            }

            return response;
        }
    }
              
}