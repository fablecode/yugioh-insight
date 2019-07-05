using System.Collections.Generic;

namespace banlistdata.application.Configuration
{
    public class ExchangeSetting
    {
        public Dictionary<string, string> Headers { get; set; }

        public byte PersistentMode { get; set; }
    }
}