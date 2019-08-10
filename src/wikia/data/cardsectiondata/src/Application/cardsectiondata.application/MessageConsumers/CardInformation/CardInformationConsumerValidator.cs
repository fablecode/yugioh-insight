using FluentValidation;

namespace cardsectiondata.application.MessageConsumers.CardInformation
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