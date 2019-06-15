using FluentValidation;

namespace article.application.ScheduledTasks.LatestBanlist
{
    public class BanlistInformationTaskValidator : AbstractValidator<BanlistInformationTask>
    {
        public BanlistInformationTaskValidator()
        {
            RuleFor(bl => bl.Category)
                .NotNull()
                .NotEmpty();

            RuleFor(bl => bl.PageSize)
                .GreaterThan(0);
        }
    }
}