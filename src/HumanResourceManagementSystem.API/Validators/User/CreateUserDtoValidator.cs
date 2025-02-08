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
                .NotEmpty().WithMessage("Email ���ର��")
                .EmailAddress().WithMessage("Email �榡�����T")
                .MaximumLength(256).WithMessage("Email ���פ���W�L 256 �Ӧr��")
                .MustNotContainXss();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("�K�X���ର��")
                .MinimumLength(8).WithMessage("�K�X�ܤֻݭn 8 �Ӧr��")
                .MaximumLength(256).WithMessage("�K�X���פ���W�L 256 �Ӧr��")
                .MustNotContainXss();

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("Roles ���ର��");

            RuleFor(x => x.Claims)
                .NotEmpty().WithMessage("Claims ���ର��");
        }
    }
}
