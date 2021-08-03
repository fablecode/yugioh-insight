﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archetypedata.application.Configuration;
using archetypedata.domain.WebPages;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using wikia.Models.Article.Details;

namespace archetypedata.infrastructure.WebPages
{
    public class ArchetypeWebPage : IArchetypeWebPage
    {
        private readonly IOptions<AppSettings> _appsettingsOptions;
        private readonly IHtmlWebPage _htmlWebPage;
        private readonly IArchetypeThumbnail _archetypeThumbnail;

        public ArchetypeWebPage(IOptions<AppSettings> appsettingsOptions, IHtmlWebPage htmlWebPage, IArchetypeThumbnail archetypeThumbnail)
        {
            _appsettingsOptions = appsettingsOptions;
            _htmlWebPage = htmlWebPage;
            _archetypeThumbnail = archetypeThumbnail;
        }

        public IEnumerable<string> Cards(Uri archetypeUrl)
        {
            var cardList = new HashSet<string>();

            var archetypeWebPage = _htmlWebPage.Load(archetypeUrl);

            var tableCollection = archetypeWebPage.DocumentNode
                .SelectNodes("//table")
                .Where(t => t.Attributes["class"] != null && t.Attributes["class"].Value.Contains("card-list"))
                .ToList();

            foreach (var tb in tableCollection)
            {
                var cardLinks = tb.SelectNodes("./tbody/tr/td[position() =1]/a");

                cardList.UnionWith(cardLinks.Select(cn => cn.InnerText));
            }

            var furtherResultsUrl = GetFurtherResultsUrl(archetypeWebPage);

            if (!string.IsNullOrEmpty(furtherResultsUrl))
            {
                if (!furtherResultsUrl.Contains("http"))
                    furtherResultsUrl = _appsettingsOptions.Value.WikiaDomainUrl + furtherResultsUrl;

                cardList.UnionWith(CardsFromFurtherResultsUrl(furtherResultsUrl));
            }

            return cardList;
        }

        public List<string> CardsFromFurtherResultsUrl(string furtherResultsUrl)
        {
            var cardList = new HashSet<string>();

            // change result set to 500
            var newUrl = furtherResultsUrl.Replace("limit%3D50", "limit%3D500");

            // sematic search page
            var sematicSearchPage = _htmlWebPage.Load(newUrl);

            var cardNameList =
                sematicSearchPage.DocumentNode.SelectNodes(
                    "//*[@id='result']/table/tbody/tr/td[1]/a");

            if (cardNameList != null)
                cardList.UnionWith(cardNameList.Select(cn => cn.InnerText));

            return cardList.ToList();
        }

        public string GetFurtherResultsUrl(HtmlDocument archetypeWebPage)
        {
            return archetypeWebPage.DocumentNode.SelectSingleNode("//span[@class='smw-table-furtherresults']/a")?.Attributes["href"].Value;
        }

        public async Task<string> ArchetypeThumbnail(long articleId, string archetypeWebPageUrl)
        {
            var thumbNail = await _archetypeThumbnail.FromArticleId((int)articleId);

            return ArchetypeThumbnail(thumbNail, archetypeWebPageUrl);
        }

        public string ArchetypeThumbnail(KeyValuePair<string, ExpandedArticle> articleDetails, string archetypeWebPageUrl)
        {
            var thumbNail = _archetypeThumbnail.FromArticleDetails(articleDetails);

            return ArchetypeThumbnail(thumbNail, archetypeWebPageUrl);
        }

        public string ArchetypeThumbnail(string thumbNailUrl, string archetypeWebPageUrl)
        {
            return string.IsNullOrWhiteSpace(thumbNailUrl)
                ? _archetypeThumbnail.FromWebPage(archetypeWebPageUrl)
                : thumbNailUrl;
        }
    }
}
