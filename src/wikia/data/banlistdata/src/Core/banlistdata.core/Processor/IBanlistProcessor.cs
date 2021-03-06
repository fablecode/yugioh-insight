﻿using System.Threading.Tasks;
using banlistdata.core.Models;

namespace banlistdata.core.Processor
{
    public interface IBanlistProcessor
    {
        Task<ArticleProcessed> Process(Article article);
    }
}