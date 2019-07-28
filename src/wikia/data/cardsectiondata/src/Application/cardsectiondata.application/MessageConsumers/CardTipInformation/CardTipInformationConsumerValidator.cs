using FluentValidation;

namespace cardsectiondata.application.MessageConsumers.CardTipInformation
{
    public class CardTipInformationConsumerValidator : AbstractValidator<CardTipInformationConsumer>
    {
        public CardTipInformationConsumerValidator()
        {
            RuleFor(ci => ci.Message)
                .NotNull()
                .NotEmpty();
        }
    }
}