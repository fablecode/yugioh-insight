using System.Threading.Tasks;
using banlistdata.core.Models;

namespace banlistdata.domain.Services.Messaging
{
    public interface IBanlistDataQueue
    {
        Task<YugiohBanlistCompletion> Publish(YugiohBanlist articleProcessed);
    }
}