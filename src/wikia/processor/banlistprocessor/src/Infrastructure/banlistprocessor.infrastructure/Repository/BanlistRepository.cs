using System;
using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;
using banlistprocessor.domain.Repository;
using banlistprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace banlistprocessor.infrastructure.Repository
{
    public class BanlistRepository : IBanlistRepository
    {
        private readonly YgoDbContext _context;

        public BanlistRepository(YgoDbContext context)
        {
            _context = context;
        }

        public Task<bool> BanlistExist(int id)
        {
            return _context.Banlist.AnyAsync(b => b.Id == id);
        }

        public async Task<Banlist> Add(Banlist banlist)
        {
            banlist.Created =
                banlist.Updated = DateTime.UtcNow;

            _context.Banlist.Add(banlist);

            await _context.SaveChangesAsync();

            return banlist;
        }

        public async Task<Banlist> Update(Banlist banlist)
        {
            banlist.Updated = DateTime.UtcNow;

            _context.Banlist.Update(banlist);

            await _context.SaveChangesAsync();

            return banlist;
        }

        public Task<Banlist> GetBanlistById(int id)
        {
            return _context
                .Banlist
                .Include(b => b.Format)
                .Include(b => b.BanlistCard)
                .AsNoTracking()
                .SingleOrDefaultAsync(b => b.Id == id);
        }
    }
}