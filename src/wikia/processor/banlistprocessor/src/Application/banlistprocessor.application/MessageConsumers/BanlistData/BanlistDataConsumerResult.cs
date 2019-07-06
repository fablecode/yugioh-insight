using System;
using System.Collections.Generic;
using System.Linq;
using banlistprocessor.core.Models;

namespace banlistprocessor.application.MessageConsumers.BanlistData
{
    public class BanlistDataConsumerResult
    {
        public bool IsSuccessful => !Errors.Any();
        public YugiohBanlist Banlist { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public Exception Exception { get; set; }
    }

}
