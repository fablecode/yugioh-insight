using System.Threading.Tasks;
using yugioh_insight.Enums;
using yugioh_insight.Models.BanlistModels;

namespace yugioh_insight.Domain
{
    public interface IBanlistManager
    {
        Task<Banlist> LatestBanlist(BanlistType banlistType);
        Task<Banlist> LatestBanlist(BanlistArticleSummary latestArticleBanlistSummary);
    }
}