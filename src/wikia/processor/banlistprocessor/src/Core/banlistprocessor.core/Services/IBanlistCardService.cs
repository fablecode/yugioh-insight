using System.Collections.Generic;
using System.Threading.Tasks;
using banlistprocessor.core.Models;
using banlistprocessor.core.Models.Db;

namespace banlistprocessor.core.Services
{
    public interface IBanlistCardService
    {
        Task<ICollection<BanlistCard>> Update(long newBanlistId, List<YugiohBanlistSection> yugiohBanlistSections);
    }
}