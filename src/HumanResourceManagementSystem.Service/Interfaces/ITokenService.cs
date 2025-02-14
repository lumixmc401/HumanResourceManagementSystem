using HumanResourceManagementSystem.Service.DTOs.Token;
using HumanResourceManagementSystem.Service.DTOs.User;

namespace HumanResourceManagementSystem.Service.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponseDto> GenerateTokensAsync(AuthenticationResultDto authResult, string deviceInfo);
        Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequest request, string deviceInfo);
        Task RevokeRefreshTokenAsync(RevokeRefreshTokenRequest request);
        Task BlacklistAccessTokenAsync(string accessToken);
        Task<bool> IsAccessTokenBlacklistedAsync(string accessToken);
    }
}
