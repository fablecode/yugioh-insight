using article.core.Enums;
using HtmlAgilityPack;

namespace article.domain.WebPages.Banlists
{
    public interface IBanlistHtmlDocument
    {
        HtmlNode GetBanlistHtmlNode(BanlistType banlistType, string banlistUrl);
        HtmlNode GetBanlistHtmlNode(BanlistType banlistType, HtmlDocument document);
    }
}