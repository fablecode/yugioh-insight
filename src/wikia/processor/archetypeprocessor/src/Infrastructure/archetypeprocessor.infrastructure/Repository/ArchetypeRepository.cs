using archetypeprocessor.core.Models.Db;
using archetypeprocessor.domain.Repository;
using archetypeprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace archetypeprocessor.infrastructure.Repository
{
    public class ArchetypeRepository : IArchetypeRepository
    {
        private readonly YgoDbContext _dbContext;

        public ArchetypeRepository(YgoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Archetype> ArchetypeById(long id)
        {
            return _dbContext
                    .Archetype
                    .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Archetype> Add(Archetype archetype)
        {
            await _dbContext.Archetype.AddAsync(archetype);
            await _dbContext.SaveChangesAsync();

            return archetype;
        }

        public async Task<Archetype> Update(Archetype archetype)
        {
            _dbContext.Archetype.Update(archetype);

            await _dbContext.SaveChangesAsync();

            return archetype;
        }
    }
}