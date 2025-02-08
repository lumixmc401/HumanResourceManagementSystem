using FluentValidation;
using HumanResourceManagementSystem.Service.DTOs.User;
using BuildingBlock.Common.Extensions.FluentValidation;

namespace HumanResourceManagementSystem.Api.Validators.User
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email 不能為空")
                .EmailAddress().WithMessage("Email 格式不正確")
                .MaximumLength(256).WithMessage("Email 長度不能超過 256 個字元")
                .MustNotContainXss();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("密碼不能為空")
                .MinimumLength(8).WithMessage("密碼至少需要 8 個字元")
                .MaximumLength(256).WithMessage("密碼長度不能超過 256 個字元")
                .MustNotContainXss();

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("Roles 不能為空");

            RuleFor(x => x.Claims)
                .NotEmpty().WithMessage("Claims 不能為空");
        }
    }
}
