using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;
using banlistprocessor.domain.Repository;
using banlistprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace banlistprocessor.infrastructure.Repository
{
    public class FormatRepository : IFormatRepository
    {
        private readonly YgoDbContext _dbContext;

        public FormatRepository(YgoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Format> FormatByAcronym(string acronym)
        {
            return _dbContext
                .Format
                .AsNoTracking()
                .SingleOrDefaultAsync(f => f.Acronym == acronym);
        }
    }
}