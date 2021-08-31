using Cards.Domain.Enums;

namespace Cards.Domain.Models
{
    public class Card
    {
        public string Name { get; }

        protected Card(string name)
        {
            Name = name;
        }
    }
}