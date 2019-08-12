using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.domain.Repository;
using cardsectionprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace cardsectionprocessor.infrastructure.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly YgoDbContext _context;

        public CardRepository(YgoDbContext context)
        {
            _context = context;
        }

        public Task<Card> CardByName(string name)
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
                    .SingleOrDefaultAsync(c => c.Name == name);
        }
    }
}