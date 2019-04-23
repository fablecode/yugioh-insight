using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardprocessor.domain.Repository;
using cardprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Attribute = cardprocessor.core.Models.Db.Attribute;

namespace cardprocessor.infrastructure.Repository
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly YgoDbContext _dbContext;

        public AttributeRepository(YgoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Attribute>> AllAttributes()
        {
            return _dbContext.Attribute.OrderBy(c => c.Name).ToListAsync();
        }

    }
}