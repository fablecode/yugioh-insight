using System.Linq;
using FluentValidation;

namespace article.application.ScheduledTasks.Archetype
{
    public class ArchetypeInformationTaskValidator : AbstractValidator<ArchetypeInformationTask>
    {
        public ArchetypeInformationTaskValidator()
        {
            RuleFor(ci => ci.Categories)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .Must(ci => ci.All(c => !string.IsNullOrWhiteSpace(c)))
                .WithMessage("All {PropertyName} must be valid.");

            RuleFor(ci => ci.PageSize)
                .GreaterThan(0);
        }
    }
}