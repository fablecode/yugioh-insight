using System;
using banlistdata.core.Enums;

namespace banlistdata.core.Models
{
    public class BanlistArticleSummary
    {
        public int ArticleId { get; set; }
        public BanlistType BanlistType { get; set; }
        public DateTime StartDate { get; set; }
    }
}