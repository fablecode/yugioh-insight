using System.Collections.Generic;

namespace article.application.Configuration
{
    public record QueueSetting
    {
        public Dictionary<string, string> Headers { get; init; }

        public byte PersistentMode { get; init; }
    }
}