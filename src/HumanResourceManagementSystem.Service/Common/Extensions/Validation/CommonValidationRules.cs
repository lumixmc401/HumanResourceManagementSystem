using BuildingBlock.Core.Common.Extensions.FluentValidation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Common.Extensions.Validation
{
    public static class CommonValidationRules
    {
        public static IRuleBuilder<T, string> ValidatePassword<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("密碼不能為空")
                .MinimumLength(8).WithMessage("密碼至少需要 8 個字元")
                .MaximumLength(256).WithMessage("密碼長度不能超過 256 個字元")
                .MustNotContainXss();
        }

        public static IRuleBuilder<T, string> ValidateEmail<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .EmailAddress().WithMessage("Email 格式不正確")
                .MaximumLength(256).WithMessage("Email 長度不能超過 256 個字元")
                .MustNotContainXss();
        }
    }
}
