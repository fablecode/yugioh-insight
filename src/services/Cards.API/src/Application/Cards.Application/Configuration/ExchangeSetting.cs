using System.Collections.Generic;

namespace Cards.Application.Configuration
{
    public record ExchangeSetting
    {
        public Dictionary<string, string> Headers { get; init; }

        public byte PersistentMode { get; init; }
    }
}