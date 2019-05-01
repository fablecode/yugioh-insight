using cardprocessor.application.Models.Cards.Input;
using FluentValidation;

namespace cardprocessor.application.Validations.Cards
{
    public class CardValidator : AbstractValidator<CardInputModel>
    {
        public CardValidator()
        {
            RuleFor(c => c.CardType)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .IsInEnum();

            RuleFor(c => c).SetValidator(new MonsterCardValidator());
            RuleFor(c => c).SetValidator(new SpellCardValidator());
            RuleFor(c => c).SetValidator(new TrapCardValidator());
        }
    }
}