using FluentValidation;
using Ganss.Xss;

namespace BuildingBlock.Common.Extensions.FluentValidation
{
    public static class MustNotContainXssExtemsion
    {
        private static readonly HtmlSanitizer _sanitizer = new();

        static MustNotContainXssExtemsion()
        {
        }
        public static IRuleBuilderOptions<T, string> MustNotContainXss<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => IsSafeHtml(x)).WithMessage("{PropertyName} �]�t���w�����r��");
        }

        private static bool IsSafeHtml(string input)
        {
            var sanitized = _sanitizer.Sanitize(input);
            return sanitized == input;
        }
    }
}