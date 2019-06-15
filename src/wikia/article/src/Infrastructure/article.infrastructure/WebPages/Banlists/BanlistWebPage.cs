using System;
using System.Collections.Generic;
using System.Linq;
using article.application.Configuration;
using article.core.Enums;
using article.domain.WebPages;
using article.domain.WebPages.Banlists;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace article.infrastructure.WebPages.Banlists
{
    public class BanlistWebPage : IBanlistWebPage
    {
        private readonly IHtmlWebPage _htmlWebPage;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly IBanlistHtmlDocument _banlistHtmlDocument;

        public BanlistWebPage(IHtmlWebPage htmlWebPage, IOptions<AppSettings> appSettingsOptions, IBanlistHtmlDocument banlistHtmlDocument)
        {
            _htmlWebPage = htmlWebPage;
            _appSettingsOptions = appSettingsOptions;
            _banlistHtmlDocument = banlistHtmlDocument;
        }

        public Dictionary<string, List<Uri>> GetBanlistUrlList(BanlistType banlistType, string banlistUrl)
        {
            var banlistUrlListNode = _banlistHtmlDocument.GetBanlistHtmlNode(banlistType, banlistUrl);

            return GetBanlistUrlList(banlistUrlListNode);
        }

        public Dictionary<string, List<Uri>> GetBanlistUrlList(HtmlNode banlistUrlListNode)
        {
            var banlistUrlsByYear = new Dictionary<string, List<Uri>>();

            foreach (var li in banlistUrlListNode.SelectNodes("li"))
            {
                var yearNode = li.SelectSingleNode("a");

                if (yearNode != null)
                {
                    var yearBanlists = new List<Uri>();

                    var year = yearNode.InnerText;

                    var liTags = li.SelectNodes("ul/li");

                    if (liTags != null)
                    {
                        foreach (var banlistLink in liTags)
                        {
                            var aTag = banlistLink.SelectSingleNode("a");

                            if (aTag != null)
                                yearBanlists.Add(new Uri(_appSettingsOptions.Value.WikiaDomainUrl + aTag.Attributes["href"].Value));
                        }

                        if(yearBanlists.Any())
                            banlistUrlsByYear.Add(year, yearBanlists);
                    }
                }
            }

            return banlistUrlsByYear;
        }
    }
}