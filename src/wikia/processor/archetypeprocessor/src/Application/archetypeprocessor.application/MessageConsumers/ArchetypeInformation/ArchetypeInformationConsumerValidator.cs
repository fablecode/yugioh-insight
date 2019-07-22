using FluentValidation;

namespace archetypeprocessor.application.MessageConsumers.ArchetypeInformation
{
    public class ArchetypeInformationConsumerValidator : AbstractValidator<ArchetypeInformationConsumer>
    {
        public ArchetypeInformationConsumerValidator()
        {
            RuleFor(ci => ci.Message)
                .NotNull()
                .NotEmpty();
        }
    }
}