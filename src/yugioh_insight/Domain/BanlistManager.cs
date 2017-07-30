using System;
using System.Linq;
using System.Threading.Tasks;
using yugioh_insight.Domain.Client;
using yugioh_insight.Enums;
using yugioh_insight.Helpers;
using yugioh_insight.Models.BanlistModels;

namespace yugioh_insight.Domain
{
    public class BanlistManager : IBanlistManager
    {
        private readonly IYugiohClient _client;

        public BanlistManager()
            : this(new YugiohClient())
        {
            
        }

        public BanlistManager(IYugiohClient client)
        {
            _client = client;
        }

        public async Task<Banlist> LatestBanlist(BanlistType banlistType)
        {
            var banlistArticles = await _client.ArticlePageList(ConfigSettings.BanlistCategory);

            var articleIds = banlistArticles.Items.Select(i => i.Id).ToArray();

            var articleDetails = await _client.ArticleDetails(articleIds);

            var banlistArticleSummaries = articleDetails.Items.Select(a => BanlistHelpers.ExtractBanlistArticleDetails(a.Value.Abstract, a.Key)).Where(a => a != null).ToList();

            var latestArticleBanlistSummary = banlistArticleSummaries.OrderByDescending(b => b.StartDate).FirstOrDefault(b => b.BanlistType == banlistType);

            var banlist = await LatestBanlist(latestArticleBanlistSummary);

            return banlist;
        }

        public async Task<Banlist> LatestBanlist(BanlistArticleSummary latestArticleBanlistSummary)
        {
            if(latestArticleBanlistSummary == null)
                throw new ArgumentNullException(nameof(latestArticleBanlistSummary));

            if(latestArticleBanlistSummary.ArticleId <= 0)
                throw new ArgumentException("Parameter 'articleId' is invalid", nameof(latestArticleBanlistSummary.ArticleId));

            const char beginChar = '「';
            const char endChar = '」';

            var response = new Banlist
            {
                BanlistType = latestArticleBanlistSummary.BanlistType,
                StartDate = latestArticleBanlistSummary.StartDate
            };

            var banlistContentResult = await _client.ArticleSimple(latestArticleBanlistSummary.ArticleId);

            foreach (var section in banlistContentResult.Sections)
            {
                // skip references section
                if (section.Title.ToLower() == "references")
                    continue;

                //new section
                var bs = new BanlistSection
                {
                    Title = StringHelpers.RemoveBetween(section.Title, beginChar, endChar).Trim(),
                    Content = ContentResultHelpers.GetSectionContentList(section).OrderBy(c => c).ToList()
                };

                // remove invalid characters
                if (bs.Content.Any())
                    bs.Content = bs.Content.Select(c => StringHelpers.RemoveBetween(c, beginChar, endChar)).ToList();

                response.Sections.Add(bs);
            }

            return response;
        }
    }
}