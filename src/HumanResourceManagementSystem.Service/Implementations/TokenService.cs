// src/HumanResourceManagementSystem.Service/Implementations/TokenService.cs
using BuildingBlock.Security.Jwt;
using HumanResourceManagementSystem.Service.DTOs.Token;
using HumanResourceManagementSystem.Service.DTOs.User;
using HumanResourceManagementSystem.Service.Exceptions.Token;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace HumanResourceManagementSystem.Service.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly ITokenCacheService _tokenCache;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private const int RefreshTokenExpiryDays = 7;
        private const string BlacklistPrefix = "blacklist:";
        private const int AccessTokenBlacklistTTL = 600; // 10 minutes

        public TokenService(
            ITokenCacheService tokenCache,
            IJwtTokenGenerator jwtGenerator)
        {
            _tokenCache = tokenCache;
            _jwtGenerator = jwtGenerator;
        }
        public async Task BlacklistAccessTokenAsync(string accessToken)
        {
            var key = $"{BlacklistPrefix}{accessToken}";
            await _tokenCache.SetAsync(key, "revoked", TimeSpan.FromSeconds(AccessTokenBlacklistTTL));
        }

        public async Task<bool> IsAccessTokenBlacklistedAsync(string accessToken)
        {
            var key = $"{BlacklistPrefix}{accessToken}";
            return await _tokenCache.ExistsAsync(key);
        }

        public async Task<TokenResponseDto> GenerateTokensAsync(
            AuthenticationResultDto authResult,
            string deviceInfo)
        {
            string accessToken = _jwtGenerator.GenerateJwtToken(
                authResult.UserId,
                authResult.UserName,
                authResult.RoleIds);

            string refreshToken = GenerateRefreshToken();
            var expiry = TimeSpan.FromDays(RefreshTokenExpiryDays);

            var cacheData = new TokenCacheDto(
                authResult.UserId,
                authResult.UserName,
                authResult.RoleIds,
                refreshToken,
                DateTime.UtcNow,
                deviceInfo);

            await _tokenCache.SetRefreshTokenAsync(refreshToken, cacheData, expiry);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(10),
                RefreshTokenExpiration = DateTime.UtcNow.Add(expiry)
            };
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(
            RefreshTokenRequest request,
            string deviceInfo)
        {
            var cacheData = await _tokenCache.GetRefreshTokenAsync(request.RefreshToken) 
                ?? throw new InvalidRefreshTokenException("Invalid Refresh Token");

            await _tokenCache.RemoveRefreshTokenAsync(request.RefreshToken);

            return await GenerateTokensAsync(
                new AuthenticationResultDto
                {
                    UserId = cacheData.UserId,
                    UserName = cacheData.UserName,
                    RoleIds = cacheData.RoleIds,
                },
                deviceInfo);
        }

        public async Task RevokeRefreshTokenAsync(RevokeRefreshTokenRequest request)
        {
            var exists = await _tokenCache.GetRefreshTokenAsync(request.RefreshToken)
                ?? throw new InvalidRefreshTokenException("Invalid Refresh Token");

            await _tokenCache.RemoveRefreshTokenAsync(request.RefreshToken);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

