using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;
using banlistprocessor.domain.Repository;
using banlistprocessor.infrastructure.Database;

namespace banlistprocessor.infrastructure.Repository
{
    public class BanlistCardRepository : IBanlistCardRepository
    {
        private readonly YgoDbContext _context;

        public BanlistCardRepository(YgoDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<BanlistCard>> Update(long banlistId, IList<BanlistCard> banlistCards)
        {
            var existingBanlistCards = _context.BanlistCard.Select(bl => bl).Where(bl => bl.BanlistId == banlistId).ToList();

            if (existingBanlistCards.Any())
            {
                _context.BanlistCard.RemoveRange(existingBanlistCards);
                await _context.SaveChangesAsync();
            }

            await _context.BanlistCard.AddRangeAsync(banlistCards);
            await _context.SaveChangesAsync();
            return banlistCards;
        }
    }
}