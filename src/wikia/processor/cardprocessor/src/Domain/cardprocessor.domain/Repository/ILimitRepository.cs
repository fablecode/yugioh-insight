using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;

namespace cardprocessor.domain.Repository
{
    public interface ILimitRepository
    {
        Task<List<Limit>> AllLimits();
    }
}