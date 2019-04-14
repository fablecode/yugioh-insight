using System;

namespace articledata.application.Configuration
{
    public class RabbitMqSettings
    {
        public Uri Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}