﻿using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;

namespace cardsectionprocessor.domain.Repository
{
    public interface ICardTipRepository
    {
        Task DeleteByCardId(long id);
        Task Update(List<TipSection> tipSections);
    }
}