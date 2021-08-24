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

    public interface ISpellCardType
    {
        string Name { get; }
        string Type { get; }
    }

    public interface ITrapCardType
    {
        string Name { get; }
        string Type { get; }
    }

    public interface IMonsterCard
    {
        string Name { get; }
        string Attribute { get; }
        string Type { get; }
        string[] SubTypes { get; }
    }

    public interface IAttackAndDefence
    {
        int? Atk { get; }
        int? Def { get; }
    }

    public interface ICard
    {
        ISpellCardType SpellCardType { get; }
        string Name { get; }
    }


    public enum CardType
    {
        Monster,
        Spell,
        Trap
    }
}