using banlistdata.core.Exceptions;

namespace banlistdata.core.Models
{
    public class YugiohBanlistCompletion
    {
        public bool IsSuccessful { get; set; }
        public Article Article { get; set; }
        public YugiohBanlist Banlist { get; set; }
        public BanlistException Exception { get; set; }
    }
}