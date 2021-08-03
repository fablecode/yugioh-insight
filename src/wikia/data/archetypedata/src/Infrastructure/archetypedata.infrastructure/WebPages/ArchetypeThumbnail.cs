using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archetypedata.application.Configuration;
using archetypedata.domain.Helpers;
using archetypedata.domain.WebPages;
using Microsoft.Extensions.Options;
using wikia.Api;
using wikia.Models.Article.Details;

namespace archetypedata.infrastructure.WebPages
{
    public class ArchetypeThumbnail : IArchetypeThumbnail
    {
        private readonly IWikiArticle _wikiArticle;
        private readonly IHtmlWebPage _htmlWebPage;
        private readonly IOptions<AppSettings> _appsettings;

        public ArchetypeThumbnail(IWikiArticle wikiArticle, IHtmlWebPage htmlWebPage, IOptions<AppSettings> appsettings)
        {
            _wikiArticle = wikiArticle;
            _htmlWebPage = htmlWebPage;
            _appsettings = appsettings;
        }

        public async Task<string> FromArticleId(int articleId)
        {
            var profileDetailsList = await _wikiArticle.Details(articleId);
            var profileDetails = profileDetailsList.Items.First();

            return FromArticleDetails(profileDetails);
        }

        public string FromArticleDetails(KeyValuePair<string, ExpandedArticle> articleDetails)
        {
            return ImageHelper.ExtractImageUrl(articleDetails.Value.Thumbnail);
        }

        public string FromWebPage(string url)
        {
            var archetypeWebPage = _htmlWebPage.Load(_appsettings.Value.WikiaDomainUrl + url);

            var srcElement = archetypeWebPage.DocumentNode.SelectSingleNode("//img[@class='pi-image-thumbnail']");

            var srcAttribute = srcElement?.Attributes?["src"].Value;

            return srcAttribute != null ? ImageHelper.ExtractImageUrl(srcAttribute) : null;
        }
    }
}