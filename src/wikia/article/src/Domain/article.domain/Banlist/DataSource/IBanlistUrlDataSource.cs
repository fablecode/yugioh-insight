using System.Collections.Generic;
using article.core.Enums;

namespace article.domain.Banlist.DataSource
{
    public interface IBanlistUrlDataSource
    {
        IDictionary<int, List<int>> GetBanlists(BanlistType banlistType, string banlistUrl);
    }
}