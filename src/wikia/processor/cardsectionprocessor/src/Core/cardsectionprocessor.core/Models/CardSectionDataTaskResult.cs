using System.Collections.Generic;
using System.Linq;

namespace cardsectionprocessor.core.Models
{
    public class CardSectionDataTaskResult<T>
    {
        public bool IsSuccessful => !Errors.Any();

        public T CardSectionData { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}