using Cards.Domain.Enums;

namespace Cards.Domain.Models
{
    public sealed class SpellCard : Card
    {
        public SpellCard(string name, SpellCardType spellCardType) : base(name)
        {
            SpellCardType = spellCardType;
        }

        public SpellCardType SpellCardType { get; }
    }
}