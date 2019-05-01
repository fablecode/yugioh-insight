using cardprocessor.application.Enums;
using cardprocessor.application.Models.Cards.Input;
using FluentValidation;

namespace cardprocessor.application.Validations.Cards
{
    public class SpellCardValidator : AbstractValidator<CardInputModel>
    {
        public SpellCardValidator()
        {
            When(c => c.CardType == YgoCardType.Spell, () =>
            {
                RuleFor(c => c.Name)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .CardNameValidator();
            });
        }
    }
}