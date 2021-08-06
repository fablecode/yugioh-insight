using System.Collections.Generic;

namespace carddata.application.Configuration
{
    public record RabbitMqSettings
    {
        public string Host { get; init; }
        public int Port { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string ContentType { get; init; }

        public Dictionary<string, ExchangeSetting> Exchanges { get; init; }
        public QueuesSetting Queues { get; init; }
    }
}