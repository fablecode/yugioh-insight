using FluentValidation;

namespace banlistdata.application.MessageConsumers.BanlistInformation
{
    public class BanlistInformationConsumerValidator : AbstractValidator<BanlistInformationConsumer>
    {
        public BanlistInformationConsumerValidator()
        {
            RuleFor(ci => ci.Message)
                .NotNull()
                .NotEmpty();
        }
    }
}