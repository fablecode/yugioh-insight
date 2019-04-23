using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.domain.Repository;
using cardprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace cardprocessor.infrastructure.Repository
{
    public class TypeRepository : ITypeRepository
    {
        private readonly YgoDbContext _dbContext;

        public TypeRepository(YgoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Type>> AllTypes()
        {
            return _dbContext.Type.OrderBy(t => t.Name).ToListAsync();
        }
    }
}