using System.Collections.Generic;
using System.Linq;

namespace banlistdata.application.MessageConsumers.BanlistInformation
{
    public class BanlistInformationConsumerResult
    {
        public bool IsSuccessful => !Errors.Any();

        public string Message { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}