using System.Threading.Tasks;
using banlistprocessor.core.Models;
using banlistprocessor.core.Models.Db;

namespace banlistprocessor.core.Services
{
    public interface IBanlistService
    {
        Task<bool> BanlistExist(int id);
        Task<Banlist> Add(YugiohBanlist any);
        Task<Banlist> Update(YugiohBanlist any);
    }
}