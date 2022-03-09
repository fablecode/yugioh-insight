using System.Collections.Generic;

namespace article.domain.Settings
{
    public record ExchangeSetting
    {
        public Dictionary<string, string> Headers { get; init; }

        public byte PersistentMode { get; init; }
    }
}