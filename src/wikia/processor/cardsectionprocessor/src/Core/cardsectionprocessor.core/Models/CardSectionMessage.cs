using System.Collections.Generic;

namespace cardsectionprocessor.core.Models
{
    public class CardSectionMessage
    {
        public string Name { get; set; }
        public List<CardSection> CardSections { get; set; } = new List<CardSection>();
    }
}