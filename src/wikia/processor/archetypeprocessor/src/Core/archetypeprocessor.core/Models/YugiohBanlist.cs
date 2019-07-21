using System;
using System.Collections.Generic;
using archetypeprocessor.core.Enums;

namespace archetypeprocessor.core.Models
{
    public class YugiohBanlist
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }

        public BanlistType BanlistType { get; set; }

        public DateTime StartDate { get; set; }
        public List<YugiohBanlistSection> Sections { get; set; } = new List<YugiohBanlistSection>();
    }
}