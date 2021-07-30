using System.Linq;
using FluentValidation;

namespace article.application.ScheduledTasks.Archetype
{
    public class ArchetypeInformationTaskValidator : AbstractValidator<ArchetypeInformationTask>
    {
        public ArchetypeInformationTaskValidator()
        {
            RuleFor(ci => ci.Category)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty();

            RuleFor(ci => ci.PageSize)
                .GreaterThan(0);
        }
    }
}