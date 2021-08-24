using Cards.Domain.Enums;

namespace Cards.Domain.Models
{
    public sealed class TrapCard : Card
    {
        public TrapCard(string name, TrapCardType trapCardType) : base(name)
        {
            TrapCardType = trapCardType;
        }

        public TrapCardType TrapCardType { get; }
    }
}
