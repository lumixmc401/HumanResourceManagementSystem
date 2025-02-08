using FluentValidation;
using BuildingBlock.Common.Extensions.FluentValidation;
using HumanResourceManagementSystem.Service.DTOs.User;
using HumanResourceManagementSystem.Service.Common.Extensions.Validation;

namespace HumanResourceManagementSystem.Service.Validators.User
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).ValidateEmail();

            RuleFor(x => x.Password).ValidateEmail();

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("Roles ���ର��");

            RuleFor(x => x.Claims)
                .NotEmpty().WithMessage("Claims ���ର��");
        }
    }
}
