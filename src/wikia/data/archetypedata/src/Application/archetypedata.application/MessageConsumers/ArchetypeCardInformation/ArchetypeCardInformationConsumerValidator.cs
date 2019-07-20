using FluentValidation;

namespace archetypedata.application.MessageConsumers.ArchetypeCardInformation
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