using FluentValidation;

namespace article.application.ScheduledTasks.Articles
{
    public class ArticleInformationTaskValidator : AbstractValidator<ArticleInformationTask>
    {
        public ArticleInformationTaskValidator()
        {
            RuleFor(bl => bl.Category)
                .NotNull()
                .NotEmpty();

            RuleFor(bl => bl.PageSize)
                .GreaterThan(0);
        }
    }
}