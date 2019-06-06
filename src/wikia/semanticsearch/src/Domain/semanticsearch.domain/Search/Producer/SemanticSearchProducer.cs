using HtmlAgilityPack;
using semanticsearch.core.Model;
using semanticsearch.core.Search;
using semanticsearch.domain.WebPage;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace semanticsearch.domain.Search.Producer
{
    public class SemanticSearchProducer : ISemanticSearchProducer
    {
        private readonly IHtmlWebPage _htmlWebPage;

        public SemanticSearchProducer(IHtmlWebPage htmlWebPage)
        {
            _htmlWebPage = htmlWebPage;
        }

        public async Task Producer(string url, ITargetBlock<SemanticCard> targetBlock)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException(nameof(url));

            if (targetBlock == null)
                throw new ArgumentException(nameof(targetBlock));

            var uri = new Uri(url);

            HtmlNode nextLink;
            var nextPageUrl = url;

            do
            {
                var doc = _htmlWebPage.Load(nextPageUrl);

                var tableRows = doc.DocumentNode.SelectNodes("//table[@class='sortable wikitable smwtable']/tbody/tr") ?? doc.DocumentNode.SelectNodes("//table[@class='sortable wikitable smwtable card-list']/tbody/tr");

                foreach (var row in tableRows)
                {
                    var semanticCard = new SemanticCard
                    {
                        Name = row.SelectSingleNode("td[position() = 1]")?.InnerText.Trim(),
                        Url = row.SelectSingleNode("td[position() = 1]/a")?.Attributes["href"]?.Value,
                    };

                    if (!string.IsNullOrWhiteSpace(semanticCard.Name))
                        await targetBlock.SendAsync(semanticCard);
                }

                nextLink = doc.DocumentNode.SelectSingleNode("//a[contains(text(), 'Next')]");

                if (nextLink != null)
                {
                    var hrefLink = $"{uri.Host}{nextLink.Attributes["href"].Value}";

                    hrefLink = WebUtility.HtmlDecode(hrefLink);

                    nextPageUrl = hrefLink;
                }

            } while (nextLink != null);

            // Signals no more messages to produced or accepted.
            targetBlock.Complete();
        }
    }
}