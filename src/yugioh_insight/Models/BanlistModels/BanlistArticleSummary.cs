using System;
using yugioh_insight.Enums;

namespace yugioh_insight.Models.BanlistModels
{
    public class BanlistArticleSummary
    {
        public int ArticleId { get; set; }
        public BanlistType? BanlistType { get; set; }
        public DateTime? StartDate { get; set; }
    }
}