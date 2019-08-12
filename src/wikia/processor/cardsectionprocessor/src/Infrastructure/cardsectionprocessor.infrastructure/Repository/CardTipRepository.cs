using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.domain.Repository;
using cardsectionprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace cardsectionprocessor.infrastructure.Repository
{
    public class CardTipRepository : ICardTipRepository
    {
        private readonly YgoDbContext _context;

        public CardTipRepository(YgoDbContext context)
        {
            _context = context;
        }

        public async Task DeleteByCardId(long cardId)
        {
            var tipSections = await _context
                                .TipSection
                                .Include(t => t.Card)
                                .Include(t => t.Tip)
                                .Where(ts => ts.CardId == cardId)
                                .ToListAsync();

            if (tipSections.Any())
            {
                _context.Tip.RemoveRange(tipSections.SelectMany(t => t.Tip));
                _context.TipSection.RemoveRange(tipSections);

                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(List<TipSection> tipSections)
        {
            _context.TipSection.UpdateRange(tipSections);
            await _context.SaveChangesAsync();
        }
    }
}