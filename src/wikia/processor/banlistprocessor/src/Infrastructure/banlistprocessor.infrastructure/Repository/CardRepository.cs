using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;
using banlistprocessor.domain.Repository;
using banlistprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace banlistprocessor.infrastructure.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly YgoDbContext _context;

        public CardRepository(YgoDbContext context)
        {
            _context = context;
        }

        public Task<Card> CardByName(string cardName)
        {
            return _context
                .Card
                .Include(c => c.CardSubCategory)
                .ThenInclude(sc => sc.SubCategory)
                .Include(c => c.CardAttribute)
                .ThenInclude(ca => ca.Attribute)
                .Include(c => c.CardType)
                .ThenInclude(ct => ct.Type)
                .Include(c => c.CardLinkArrow)
                .ThenInclude(cla => cla.LinkArrow)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Name == cardName);
        }
    }
}