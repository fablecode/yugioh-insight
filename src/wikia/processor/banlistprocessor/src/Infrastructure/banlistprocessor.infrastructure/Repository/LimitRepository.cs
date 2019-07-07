using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;
using banlistprocessor.domain.Repository;
using banlistprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace banlistprocessor.infrastructure.Repository
{
    public class LimitRepository : ILimitRepository
    {
        private readonly YgoDbContext _dbContext;

        public LimitRepository(YgoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Limit>> GetAll()
        {
            return _dbContext.Limit.OrderBy(c => c.Name).ToListAsync();
        }
    }
}