using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using banlistprocessor.core.Models;
using banlistprocessor.core.Models.Db;
using banlistprocessor.core.Services;
using banlistprocessor.domain.Repository;

namespace banlistprocessor.domain.Services
{
    public class BanlistCardService : IBanlistCardService
    {
        private readonly ILimitRepository _limitRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IBanlistCardRepository _banlistCardRepository;

        public BanlistCardService(ILimitRepository limitRepository, ICardRepository cardRepository, IBanlistCardRepository banlistCardRepository)
        {
            _limitRepository = limitRepository;
            _cardRepository = cardRepository;
            _banlistCardRepository = banlistCardRepository;
        }

        public async Task<ICollection<BanlistCard>> Update(long banlistId, List<YugiohBanlistSection> yugiohBanlistSections)
        {
            var banlistCards = await MapToBanlistCards(banlistId, yugiohBanlistSections);
            return await _banlistCardRepository.Update(banlistId, banlistCards);
        }


        #region private helpers

        private async Task<IList<BanlistCard>> MapToBanlistCards(long banlistId, IList<YugiohBanlistSection> yugiohBanlistSections)
        {
            var banlistCards = new List<BanlistCard>();

            var cardlimits = await _limitRepository.GetAll();

            const string forbidden = "forbidden";
            const string limited = "limited";
            const string semiLimited = "semi-limited";
            const string unlimited = "unlimited";

            var forbiddenBanSection = yugiohBanlistSections.SingleOrDefault(bs => bs.Title.Equals(forbidden, StringComparison.OrdinalIgnoreCase));
            var limitedBanSection = yugiohBanlistSections.SingleOrDefault(bs => bs.Title.Equals(limited, StringComparison.OrdinalIgnoreCase));
            var semiLimitedBanSection = yugiohBanlistSections.SingleOrDefault(bs => bs.Title.Equals(semiLimited, StringComparison.OrdinalIgnoreCase));
            var unlimitedBanSection = yugiohBanlistSections.SingleOrDefault(bs => bs.Title.Equals(unlimited, StringComparison.OrdinalIgnoreCase));

            if (forbiddenBanSection != null && forbiddenBanSection.Content.Any())
                await AddCardsToBanlist(banlistCards, forbiddenBanSection, banlistId, cardlimits, forbidden);

            if (limitedBanSection != null && limitedBanSection.Content.Any())
                await AddCardsToBanlist(banlistCards, limitedBanSection, banlistId, cardlimits, limited);

            if (semiLimitedBanSection != null && semiLimitedBanSection.Content.Any())
                await AddCardsToBanlist(banlistCards, semiLimitedBanSection, banlistId, cardlimits, semiLimited);

            if (unlimitedBanSection != null && unlimitedBanSection.Content.Any())
                await AddCardsToBanlist(banlistCards, unlimitedBanSection, banlistId, cardlimits, unlimited);

            return banlistCards;
        }

        private async Task AddCardsToBanlist(List<BanlistCard> banlistCards, YugiohBanlistSection forbiddenBanSection, long banlistId, IEnumerable<Limit> cardlimits, string limit)
        {
            var selectedLimit = cardlimits.Single(l => l.Name.Equals(limit, StringComparison.OrdinalIgnoreCase));

            if (selectedLimit != null)
            {
                foreach (var cardName in forbiddenBanSection.Content)
                {
                    var card = await _cardRepository.CardByName(cardName);

                    if (card != null && !banlistCards.Any(blc => blc.BanlistId == banlistId && blc.CardId == card.Id))
                        banlistCards.Add(new BanlistCard { BanlistId = banlistId, CardId = card.Id, LimitId = selectedLimit.Id });
                }
            }
        }

        #endregion
    }

}
