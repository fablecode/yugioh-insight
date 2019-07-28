using System.Collections.Generic;

namespace cardsectiondata.core.Models
{
    public class CardSection
    {
        public string Name { get; set; }
        public List<string> ContentList { get; set; } = new List<string>();
    }

    public class CardSectionMessage
    {
        public string Name { get; set; }
        public List<CardSection> CardSections { get; set; } = new List<CardSection>();
    }
}