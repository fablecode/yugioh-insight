using System;
using banlistdata.core.Models;

namespace banlistdata.core.Exceptions
{
    public class BanlistException
    {
        public YugiohBanlist Banlist { get; set; }

        public Exception Exception { get; set; }
    }
}