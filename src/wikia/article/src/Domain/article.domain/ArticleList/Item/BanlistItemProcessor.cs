using System.Linq;
using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using article.core.Constants;
using article.core.Models;
using wikia.Api;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.ArticleList.Item
{
    //public class BanlistItemProcessor : IArticleItemProcessor
    //{
    //    private readonly IWikiArticle _wikiArticle;
    //    private readonly IYugiohBanlistService _banlistService;

    //    public BanlistItemProcessor(IWikiArticle wikiArticle, IYugiohBanlistService banlistService)
    //    {
    //        _wikiArticle = wikiArticle;
    //        _banlistService = banlistService;
    //    }

    //    public bool Handles(string category)
    //    {
    //        return category == ArticleCategory.ForbiddenAndLimited;
    //    }

    //    public async Task<ArticleTaskResult> ProcessItem(UnexpandedArticle item)
    //    {
    //        var response = new ArticleTaskResult { Article = item };

    //        var articleDetailsList = await _wikiArticle.Details(item.Id);

    //        var articleDetails = articleDetailsList.Items.First();

    //        var banlistArticleSummary = BanlistHelpers.ExtractBanlistArticleDetails(articleDetails.Value.Id, articleDetails.Value.Abstract);

    //        if (banlistArticleSummary != null)
    //        {
    //            const char beginChar = '「';
    //            const char endChar = '」';

    //            var banlist = new YugiohBanlist
    //            {
    //                ArticleId = banlistArticleSummary.ArticleId,
    //                Title = articleDetails.Value.Title,
    //                BanlistType = banlistArticleSummary.BanlistType,
    //                StartDate = banlistArticleSummary.StartDate
    //            };


    //            var banlistContentResult = await _wikiArticle.Simple(banlistArticleSummary.ArticleId);

    //            foreach (var section in banlistContentResult.Sections)
    //            {
    //                // skip references section
    //                if (section.Title.ToLower() == "references")
    //                    continue;

    //                // new section
    //                var ybls = new YugiohBanlistSection
    //                {
    //                    Title = StringHelpers.RemoveBetween(section.Title, beginChar, endChar).Trim(),
    //                    Content = ContentResultHelpers.GetSectionContentList(section).OrderBy(c => c).ToList()
    //                };

    //                // remove invalid characters
    //                if (ybls.Content.Any())
    //                    ybls.Content = ybls.Content.Select(c => StringHelpers.RemoveBetween(c, beginChar, endChar)).ToList();

    //                banlist.Sections.Add(ybls);
    //            }

    //            await _banlistService.AddOrUpdate(banlist);

    //            response.Data = banlist;

    //            response.IsSuccessfullyProcessed = true;
    //        }

    //        return response;
    //    }
    //}
}