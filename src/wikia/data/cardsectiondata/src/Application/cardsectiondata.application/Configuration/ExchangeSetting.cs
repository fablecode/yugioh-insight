using System.Collections.Generic;

namespace cardsectiondata.application.Configuration
{
    public class ExchangeSetting
    {
        public Dictionary<string, QueueSetting> Queues { get; set; }

        public byte PersistentMode { get; set; }
    }
}