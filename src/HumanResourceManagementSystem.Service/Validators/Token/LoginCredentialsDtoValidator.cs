using FluentValidation;
using HumanResourceManagementSystem.Service.Common.Extensions.Validation;
using HumanResourceManagementSystem.Service.DTOs.User;

namespace HumanResourceManagementSystem.Service.Validators.Token
{
    public class LoginCredentialsDtoValidator: AbstractValidator<LoginCredentialsDto>
    {
        public LoginCredentialsDtoValidator() 
        {
            RuleFor(x => x.Email).ValidateEmail();

            RuleFor(x => x.Password).ValidatePassword();
        }
    }
}
