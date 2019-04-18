using FluentValidation;

namespace article.application.ScheduledTasks.CardInformation
{
    public class CardInformationTaskValidator : AbstractValidator<CardInformationTask>
    {
        private const int MaxPageSize = 500;

        public CardInformationTaskValidator()
        {
            RuleFor(ci => ci.Categories)
                .NotNull()
                .NotEmpty();

            RuleFor(ci => ci.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(MaxPageSize);
        }
    }
}