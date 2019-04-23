using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using cardprocessor.domain.Repository;

namespace cardprocessor.domain.Services
{
    public class LimitService : ILimitService
    {
        private readonly ILimitRepository _limitRepository;

        public LimitService(ILimitRepository limitRepository)
        {
            _limitRepository = limitRepository;
        }
        public Task<List<Limit>> AllLimits()
        {
            return _limitRepository.AllLimits();
        }
    }
}