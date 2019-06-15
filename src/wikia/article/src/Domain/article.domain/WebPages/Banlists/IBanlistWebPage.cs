using System;
using System.Collections.Generic;
using article.core.Enums;
using HtmlAgilityPack;

namespace article.domain.WebPages.Banlists
{
    public interface IBanlistWebPage
    {
        Dictionary<string, List<Uri>> GetBanlistUrlList(BanlistType banlistType, string banlistUrl);
        Dictionary<string, List<Uri>> GetBanlistUrlList(HtmlNode banlistUrlListNode);
    }
}