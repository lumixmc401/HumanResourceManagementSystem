using System.Security;
using BuildingBlock.Core.Common.Exceptions;
using BuildingBlock.Security.Jwt;
using HumanResourceManagementSystem.Api.Models.Response;
using HumanResourceManagementSystem.Service.DTOs.Token;
using HumanResourceManagementSystem.Service.DTOs.User;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;

namespace HumanResourceManagementSystem.Api.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public TokenController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="dto">登入憑證</param>
        /// <returns>令牌資訊</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCredentialsDto dto)
        {
            var authResult = await _userService.AuthenticateAsync(dto);
            var deviceInfo = GetDeviceInfo();
            var tokenResponse = await _tokenService.GenerateTokensAsync(authResult, deviceInfo);

            Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, new CookieOptions
            {
                Expires = tokenResponse.RefreshTokenExpiration
            });

            return Ok(new LoginResponse
            {
                AccessToken = tokenResponse.AccessToken,
                AccessTokenExpiration = tokenResponse.AccessTokenExpiration
            });
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        /// <returns>新的令牌資訊</returns>
        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = GetRefreshToken();
            var deviceInfo = GetDeviceInfo();
            var request = new RefreshTokenRequest { RefreshToken = refreshToken };
            var tokenResponse = await _tokenService.RefreshTokenAsync(request, deviceInfo);

            // 更新 Refresh Token Cookie
            Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, new CookieOptions
            {
                Expires = tokenResponse.RefreshTokenExpiration
            });

            return Ok(new LoginResponse
            {
                AccessToken = tokenResponse.AccessToken,
                AccessTokenExpiration = tokenResponse.AccessTokenExpiration
            });
        }

        /// <summary>
        /// 撤銷令牌
        /// </summary>
        /// <param name="refreshToken">要撤銷的刷新令牌</param>
        /// <returns>撤銷結果</returns>
        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            string refreshToken = GetRefreshToken();
            string accessToken = GetAccessToken();
            var request = new RevokeRefreshTokenRequest { RefreshToken = refreshToken };
            await _tokenService.RevokeRefreshTokenAsync(request);
            await _tokenService.BlacklistAccessTokenAsync(accessToken);
            Response.Cookies.Delete("RefreshToken");
            return Ok();
        }
        private string GetRefreshToken()
        {
            return Request.Cookies["RefreshToken"]
                ?? throw new UnauthorizedException("No Refresh Token Provide");
        }
        private string GetAccessToken()
        {
            return HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last()
                ?? throw new UnauthorizedException("No Access Token Provide");
        }
        private string GetDeviceInfo()
        {
            var userAgent = Request.Headers.UserAgent.ToString();
            var ipAddress = Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"].ToString()
                : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";

            return $"IP: {ipAddress}, UserAgent: {userAgent}";
        }
    }
}
