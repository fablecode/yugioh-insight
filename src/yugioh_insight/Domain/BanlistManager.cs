using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using yugioh_insight.Domain.Client;
using yugioh_insight.Enums;
using yugioh_insight.Models.BanlistModels;

namespace yugioh_insight.Domain
{
    public class BanlistManager
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

        public async Task<BanlistArticleSummary> LatestBanlist(BanlistType banlistType)
        {
            var banlistArticles = await _client.ArticlePageList(ConfigSettings.BanlistCategory);

            var articleIds = banlistArticles.Items.Select(i => i.Id).ToArray();

            var articleDetails = await _client.ArticleDetails(articleIds);

            var banlistArticleSummaries = articleDetails.Items.Select(a => BanlistHelpers.ExtractBanlistDetails(a.Value.Abstract, a.Key)).ToList();

            return banlistArticleSummaries.OrderByDescending(b => b.StartDate).FirstOrDefault(b => b.BanlistType.HasValue && b.BanlistType == banlistType);
        }
    }

    public static class BanlistHelpers
    {
        public static BanlistArticleSummary ExtractBanlistDetails(string titleText, string articleId = "0")
        {
            var response = new BanlistArticleSummary();
            var regex = new Regex(@"((?:TCG|OCG?)) in effect since ((?:Jan(?:uary)?|Feb(?:ruary)?|Mar(?:ch)?|Apr(?:il)?|May|Jun(?:e)?|Jul(?:y)?|Aug(?:ust)?|Sep(?:tember)?|Sept|Oct(?:ober)?|Nov(?:ember)?|Dec(?:ember)?)) ((?:\d{1,2}?)), ((?:\d{4}?))");

            var match = regex.Match(titleText);

            if (match.Success)
            {
                var format = (BanlistType)Enum.Parse(typeof(BanlistType), match.Groups[1].Value, true);
                var startDate = DateTime.Parse($"{match.Groups[2]} {match.Groups[3]}, {match.Groups[4]}");

                response.BanlistType = format;
                response.StartDate = startDate;
                response.ArticleId = int.Parse(articleId);
            }

            return response;
        }
    }
}