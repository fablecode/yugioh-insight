using System.Collections.Generic;

namespace article.application.Configuration
{
    public class QueueSetting
    {
        public Dictionary<string, string> Headers { get; set; }

        public byte PersistentMode { get; set; }
    }
}