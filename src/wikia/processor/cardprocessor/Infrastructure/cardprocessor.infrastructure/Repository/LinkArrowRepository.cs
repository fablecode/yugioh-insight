using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.domain.Repository;
using cardprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace cardprocessor.infrastructure.Repository
{
    public class LinkArrowRepository : ILinkArrowRepository
    {
        private readonly YgoDbContext _dbContext;

        public LinkArrowRepository(YgoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<LinkArrow>> AllLinkArrows()
        {
            return _dbContext.LinkArrow.OrderBy(la => la.Name).ToListAsync();
        }
    }
}