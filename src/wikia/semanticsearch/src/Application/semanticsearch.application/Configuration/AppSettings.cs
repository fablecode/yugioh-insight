using System.Collections.Generic;

namespace semanticsearch.application.Configuration
{
    public class AppSettings
    {
        public string CronSchedule { get; set; }
        public string WikiaDomainUrl { get; set; }
        public string LogFolder { get; set; }
        public Dictionary<string, string> CardSearchUrls { get; set; }
    }
}