using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;

namespace banlistprocessor.domain.Repository
{
    public interface IBanlistRepository
    {
        Task<bool> BanlistExist(int id);

        Task<Banlist> Add(Banlist banlist);
        Task<Banlist> Update(Banlist banlist);
        Task<Banlist> GetBanlistById(int id);
    }
}