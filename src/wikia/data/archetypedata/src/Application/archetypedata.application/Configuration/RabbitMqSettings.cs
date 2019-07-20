using System.Collections.Generic;

namespace archetypedata.application.Configuration
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContentType { get; set; }

        public Dictionary<string, ExchangeSetting> Exchanges { get; set; }

    }
}