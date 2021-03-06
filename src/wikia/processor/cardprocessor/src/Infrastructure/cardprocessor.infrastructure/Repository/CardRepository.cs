﻿using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.domain.Repository;
using cardprocessor.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace cardprocessor.infrastructure.Repository
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

        public async Task<Card> Add(Card newCard)
        {
            _context.Card.Add(newCard);

            await _context.SaveChangesAsync();

            return newCard;
        }

        public Task<Card> CardById(long id)
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
                        .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Card> Update(Card card)
        {
            _context.Card.Update(card);

            var result = await _context.SaveChangesAsync();

            return card;
        }

        public Task<bool> CardExists(long id)
        {
            return _context.Card.AsNoTracking().AnyAsync(c => c.Id == id);
        }
    }
}