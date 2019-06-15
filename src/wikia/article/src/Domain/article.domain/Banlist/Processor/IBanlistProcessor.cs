using System.Threading.Tasks;
using article.core.Enums;
using article.core.Models;

namespace article.domain.Banlist.Processor
{
    public interface IBanlistProcessor
    {
        Task<ArticleBatchTaskResult> Process(BanlistType banlistType);
    }
}