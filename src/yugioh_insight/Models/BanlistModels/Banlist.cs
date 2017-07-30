using System;
using System.Collections.Generic;
using yugioh_insight.Domain;
using yugioh_insight.Enums;

namespace yugioh_insight.Models.BanlistModels
{
    public class Banlist
    {
        public BanlistType BanlistType { get; set; }
        public DateTime StartDate { get; set; }
        public List<BanlistSection> Sections { get; set; } = new List<BanlistSection>();
    }
}