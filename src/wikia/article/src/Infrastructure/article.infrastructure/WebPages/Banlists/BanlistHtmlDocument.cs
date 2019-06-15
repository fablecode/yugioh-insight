using article.core.Enums;
using article.domain.WebPages;
using article.domain.WebPages.Banlists;
using HtmlAgilityPack;

namespace article.infrastructure.WebPages.Banlists
{
    public class BanlistHtmlDocument : IBanlistHtmlDocument
    {
        private readonly IHtmlWebPage _htmlWebPage;

        public BanlistHtmlDocument(IHtmlWebPage htmlWebPage)
        {
            _htmlWebPage = htmlWebPage;
        }

        public HtmlNode GetBanlistHtmlNode(BanlistType banlistType, string banlistUrl)
        {
            return GetBanlistHtmlNode(banlistType, _htmlWebPage.Load(banlistUrl));
        }
        public HtmlNode GetBanlistHtmlNode(BanlistType banlistType, HtmlDocument document)
        {
            return document
                .DocumentNode
                .SelectSingleNode($"//*[contains(@class,'nowraplinks navbox-subgroup')]/tr/th/i[contains(text(), '{banlistType.ToString().ToUpper()}')]")
                .ParentNode
                .ParentNode
                .SelectSingleNode("./td/table/tr[1]/td[1]/div/ul");
        }
    }
}