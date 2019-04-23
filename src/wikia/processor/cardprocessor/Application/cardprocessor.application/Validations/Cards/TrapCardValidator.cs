using cardprocessor.application.Enums;
using cardprocessor.application.Models.Cards.Input;
using FluentValidation;

namespace cardprocessor.application.Validations.Cards
{
    public class TrapCardValidator : AbstractValidator<CardInputModel>
    {
        public TrapCardValidator()
        {
            When(c => c.CardType == YgoCardType.Trap, () =>
            {
                RuleFor(c => c.Name)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .CardNameValidator();
            });
        }
    }
}