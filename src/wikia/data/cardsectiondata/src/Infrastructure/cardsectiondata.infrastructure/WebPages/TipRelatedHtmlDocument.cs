using cardsectiondata.application.Configuration;
using cardsectiondata.domain.WebPages;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace cardsectiondata.infrastructure.WebPages
{
    public class TipRelatedHtmlDocument : ITipRelatedHtmlDocument
    {
        private readonly IOptions<AppSettings> _appsettingsOptions;

        public TipRelatedHtmlDocument(IOptions<AppSettings> appsettingsOptions)
        {
            _appsettingsOptions = appsettingsOptions;
        }

        public HtmlNode GetTable(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//table[@class='sortable wikitable smwtable']");
        }

        public string GetUrl(HtmlDocument document)
        {
            var furtherResultsUrl =  document.DocumentNode.SelectSingleNode("//span[@class='smw-table-furtherresults']/a")?.Attributes["href"]?.Value;

            if (!string.IsNullOrWhiteSpace(furtherResultsUrl) && !furtherResultsUrl.Contains("http://"))
                furtherResultsUrl = _appsettingsOptions.Value.WikiaDomainUrl + furtherResultsUrl;

            return furtherResultsUrl;
        }
    }
}