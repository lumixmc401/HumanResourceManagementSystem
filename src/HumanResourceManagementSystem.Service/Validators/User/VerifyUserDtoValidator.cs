using FluentValidation;
using HumanResourceManagementSystem.Service.Common.Extensions.Validation;
using HumanResourceManagementSystem.Service.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Validators.User
{
    public class VerifyUserDtoValidator: AbstractValidator<VerifyUserDto>
    {
        public VerifyUserDtoValidator() 
        {
            RuleFor(x => x.Email).ValidateEmail();

            RuleFor(x => x.Password).ValidatePassword();
        }
    }
}
