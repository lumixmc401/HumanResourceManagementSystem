using FluentValidation;
using Ganss.Xss;

namespace HumanResourceManagementSystem.API.Validators.Extensions
{
    public static class MustNotContainXssExtemsion
    {
        private static readonly HtmlSanitizer _sanitizer = new();

        public static IRuleBuilderOptions<T, string> MustNotContainXss<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => IsSafeHtml(x)).WithMessage("{PropertyName} 包含不安全的字元");
        }

        private static bool IsSafeHtml(string input)
        {
            var sanitized = _sanitizer.Sanitize(input);
            return sanitized == input;
        }
    }
}