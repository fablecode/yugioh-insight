using System;
using System.Collections.Generic;
using banlistdata.core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace banlistdata.core.Models
{
    public class YugiohBanlist
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public BanlistType BanlistType { get; set; }

        public DateTime StartDate { get; set; }
        public List<YugiohBanlistSection> Sections { get; set; } = new List<YugiohBanlistSection>();
    }
}