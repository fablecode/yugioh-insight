using System.Collections.Generic;

namespace article.application.Configuration
{
    public class ExchangeSetting
    {
        public Dictionary<string, QueueSetting> Queues { get; set; }
    }
}