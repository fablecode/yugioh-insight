using FluentValidation;

namespace article.application.ScheduledTasks.Articles
{
    public class ArticleInformationTaskValidator : AbstractValidator<ArticleInformationTask>
    {
        private const int MaxPageSize = 500;

        public ArticleInformationTaskValidator()
        {
            RuleFor(bl => bl.Category)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty();

            RuleFor(bl => bl.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(MaxPageSize);
        }
    }
}