using System.Collections.Generic;
using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;

namespace banlistprocessor.domain.Repository
{
    public interface IBanlistCardRepository
    {
        Task<ICollection<BanlistCard>> Update(long banlistId, IList<BanlistCard> banlistCards);
    }
}