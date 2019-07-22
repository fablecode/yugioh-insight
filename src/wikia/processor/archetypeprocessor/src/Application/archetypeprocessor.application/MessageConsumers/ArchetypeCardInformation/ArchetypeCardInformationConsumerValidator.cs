using FluentValidation;

namespace archetypeprocessor.application.MessageConsumers.ArchetypeCardInformation
{
    public class ArchetypeCardInformationConsumerValidator : AbstractValidator<ArchetypeCardInformationConsumer>
    {
        public ArchetypeCardInformationConsumerValidator()
        {
            RuleFor(ci => ci.Message)
                .NotNull()
                .NotEmpty();
        }
    }
}