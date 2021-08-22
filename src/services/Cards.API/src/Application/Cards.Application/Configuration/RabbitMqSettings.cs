using System.Collections.Generic;

namespace Cards.Application.Configuration
{
    public record RabbitMqSettings
    {
        public string Host { get; init; }
        public int Port { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string ContentType { get; init; }

        public Dictionary<string, ExchangeSetting> Exchanges { get; init; }
        public Dictionary<string, QueueSetting> Queues { get; init; }
    }
}