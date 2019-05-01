using FluentValidation;

namespace cardprocessor.application.Validations
{
    public static class CardValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> CardNameValidator<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotNull()
                .NotEmpty()
                .Length(1, 255);
        }
    }
}