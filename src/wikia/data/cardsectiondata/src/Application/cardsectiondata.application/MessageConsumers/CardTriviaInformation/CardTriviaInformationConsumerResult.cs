using System.Collections.Generic;
using System.Linq;

namespace cardsectiondata.application.MessageConsumers.CardTriviaInformation
{
    public class CardTriviaInformationConsumerResult
    {
        public bool IsSuccessful => !Errors.Any();

        public string Message { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}