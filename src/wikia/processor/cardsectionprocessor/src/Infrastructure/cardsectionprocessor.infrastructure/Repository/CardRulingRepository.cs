using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.domain.Repository;
using cardsectionprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace cardsectionprocessor.infrastructure.Repository
{
    public class CardRulingRepository : ICardRulingRepository
    {
        private readonly YgoDbContext _context;

        public CardRulingRepository(YgoDbContext context)
        {
            _context = context;
        }

        public async Task DeleteByCardId(long cardId)
        {
            var rulingSections = await _context
                                .RulingSection
                                .Include(t => t.Card)
                                .Include(t => t.Ruling)
                                .Where(ts => ts.CardId == cardId)
                                .ToListAsync();

            if (rulingSections.Any())
            {
                _context.Ruling.RemoveRange(rulingSections.SelectMany(t => t.Ruling));
                _context.RulingSection.RemoveRange(rulingSections);

                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(List<RulingSection> rulingSections)
        {
            _context.RulingSection.UpdateRange(rulingSections);
            await _context.SaveChangesAsync();
        }
    }
}