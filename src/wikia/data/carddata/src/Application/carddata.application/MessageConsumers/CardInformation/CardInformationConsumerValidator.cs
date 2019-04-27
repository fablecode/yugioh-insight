using FluentValidation;

namespace carddata.application.MessageConsumers.CardInformation
{
    public class CardInformationConsumerValidator : AbstractValidator<CardInformationConsumer>
    {
        public CardInformationConsumerValidator()
        {
            RuleFor(ci => ci.Message)
                .NotNull()
                .NotEmpty();
        }
    }
}