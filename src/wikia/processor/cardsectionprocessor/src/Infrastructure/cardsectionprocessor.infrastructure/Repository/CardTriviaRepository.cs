using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.domain.Repository;
using cardsectionprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace cardsectionprocessor.infrastructure.Repository
{
    public class CardTriviaRepository : ICardTriviaRepository
    {
        private readonly YgoDbContext _context;

        public CardTriviaRepository(YgoDbContext context)
        {
            _context = context;
        }

        public async Task DeleteByCardId(long cardId)
        {
            var triviaSections = await _context
                                .TriviaSection
                                .Include(t => t.Card)
                                .Include(t => t.Trivia)
                                .Where(ts => ts.CardId == cardId)
                                .ToListAsync();

            if (triviaSections.Any())
            {
                _context.Trivia.RemoveRange(triviaSections.SelectMany(t => t.Trivia));
                _context.TriviaSection.RemoveRange(triviaSections);

                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(List<TriviaSection> triviaSections)
        {
            _context.TriviaSection.UpdateRange(triviaSections);
            await _context.SaveChangesAsync();
        }
    }
}