using System.Collections.Generic;
using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;

namespace banlistprocessor.domain.Repository
{
    public interface ILimitRepository
    {
        Task<ICollection<Limit>> GetAll();
    }
}