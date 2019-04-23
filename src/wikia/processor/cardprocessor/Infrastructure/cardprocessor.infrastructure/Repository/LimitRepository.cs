using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.domain.Repository;
using cardprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace cardprocessor.infrastructure.Repository
{
    public class LimitRepository : ILimitRepository
    {
        private readonly YgoDbContext _dbContext;

        public LimitRepository(YgoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Limit>> AllLimits()
        {
            return _dbContext.Limit.OrderBy(c => c.Name).ToListAsync();
        }
    }
}