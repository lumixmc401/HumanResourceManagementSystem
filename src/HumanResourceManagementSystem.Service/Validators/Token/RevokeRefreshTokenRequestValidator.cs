using FluentValidation;
using HumanResourceManagementSystem.Service.DTOs.Token;

namespace HumanResourceManagementSystem.Service.Validators.Token
{
    public class RevokeRefreshTokenRequestValidator : AbstractValidator<RevokeRefreshTokenRequest>
    {
        public RevokeRefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("刷新令牌不能為空");
            RuleFor(x => x.RefreshToken).MaximumLength(500).WithMessage("刷新令牌長度不能超過 500 個字符");
        }
    }
}
