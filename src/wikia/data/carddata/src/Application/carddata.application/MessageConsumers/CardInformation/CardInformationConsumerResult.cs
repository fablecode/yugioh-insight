using System.Collections.Generic;
using carddata.core.Models;

namespace carddata.application.MessageConsumers.CardInformation
{
    public class CardInformationConsumerResult
    {
        public ArticleConsumerResult ArticleConsumerResult { get; set; }

        public List<string> Errors { get; set; }
    }
}