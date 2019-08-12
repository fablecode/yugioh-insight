using FluentValidation;

namespace cardsectionprocessor.application.MessageConsumers.CardInformation
{
    public class CardInformationConsumerValidator : AbstractValidator<CardInformationConsumer>
    {
        public CardInformationConsumerValidator()
        {
            RuleFor(ci => ci.Category)
                .NotNull()
                .NotEmpty();

            RuleFor(ci => ci.Message)
                .NotNull()
                .NotEmpty();
        }
    }
}