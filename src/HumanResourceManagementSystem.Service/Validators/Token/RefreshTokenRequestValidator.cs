using FluentValidation;
using HumanResourceManagementSystem.Service.DTOs.Token;
using HumanResourceManagementSystem.Service.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Validators.Token
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("刷新令牌不能為空");
            RuleFor(x => x.RefreshToken).MaximumLength(500).WithMessage("刷新令牌長度不能超過 500 個字符");
        }
    }
}
