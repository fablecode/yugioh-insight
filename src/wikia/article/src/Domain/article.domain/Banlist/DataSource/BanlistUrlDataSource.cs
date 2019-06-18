using System.Collections.Generic;
using System.Text.RegularExpressions;
using article.core.Enums;
using article.domain.WebPages;
using article.domain.WebPages.Banlists;

namespace article.domain.Banlist.DataSource
{
    public class BanlistUrlDataSource : IBanlistUrlDataSource
    {
        private readonly IBanlistWebPage _banlistWebPage;
        private readonly IHtmlWebPage _htmlWebPage;

        public BanlistUrlDataSource(IBanlistWebPage banlistWebPage, IHtmlWebPage htmlWebPage)
        {
            _banlistWebPage = banlistWebPage;
            _htmlWebPage = htmlWebPage; 
        }

        public IDictionary<int, List<int>> GetBanlists(BanlistType banlistType, string banlistUrl)
        {
            var articleIdsList = new Dictionary<int, List<int>>();

            var articleIdRegex = new Regex("wgArticleId=([^,]*),");
            var banlistUrlsByYear = _banlistWebPage.GetBanlistUrlList(banlistType, banlistUrl);

            foreach (var (year, banlistUrls) in banlistUrlsByYear)
            {
                var banlistYear = int.Parse(year);
                var articleIds = new List<int>();

                foreach (var url in banlistUrls)
                {
                    var banlistPageHtml = _htmlWebPage.Load(url).DocumentNode.InnerHtml;

                    var match = articleIdRegex.Match(banlistPageHtml);

                    var wgArticleId = int.Parse(match.Groups[1].Value);

                    articleIds.Add(wgArticleId);
                }

                articleIdsList.Add(banlistYear, articleIds);
            }

            return articleIdsList;
        }
    }
}