﻿using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;

namespace cardsectionprocessor.core.Services
{
    public interface ICardTipService
    {
        Task DeleteByCardId(long id);
        Task Update(List<TipSection> tips);
    }
}