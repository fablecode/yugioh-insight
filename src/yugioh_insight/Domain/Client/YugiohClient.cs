using System.Threading.Tasks;
using wikia.Api;
using wikia.Models.Article;
using wikia.Models.Article.Details;
using wikia.Models.Article.PageList;
using wikia.Models.Article.Simple;

namespace yugioh_insight.Domain.Client
{
    public class YugiohClient : IYugiohClient
    {
        private readonly WikiArticle _wikiaArticleClient;

        public YugiohClient()
        {
            _wikiaArticleClient = new WikiArticle(ConfigSettings.DomainUrl);
        }

        public async Task<ExpandedListArticleResultSet> ArticlePageList(string category)
        {
            return await _wikiaArticleClient.PageList(new ArticleListRequestParameters{ Category = category, Limit = 500});
        }

        public async Task<ContentResult> ArticleSimple(int article)
        {
            return await _wikiaArticleClient.Simple(article);
        }

        public async Task<ExpandedArticleResultSet> ArticleDetails(params int[] articleIds)
        {
            return await _wikiaArticleClient.Details(articleIds);
        }
    }
}