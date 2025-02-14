using HumanResourceManagementSystem.Service.DTOs.User;
using System.Net.Http.Json;
using System.Net;
using NUnit.Framework.Internal;
using HumanResourceManagementSystem.Api.Tests.Constants.Api;
using HumanResourceManagementSystem.Api.Tests.Constants.Data;
using FluentAssertions;
using System.Net.Http.Headers;
using HumanResourceManagementSystem.Api.Models.Response;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HumanResourceManagementSystem.Api.Tests.Tests
{
    public class TokenControllerTest
    {
        private HttpClient _client;
        private static IEnumerable<TestCaseData> ValidUserCredentials
        {
            get
            {
                yield return new TestCaseData(UserConstants.Credentials.AdminEmail, UserConstants.Credentials.AdminPassword);
                yield return new TestCaseData(UserConstants.Credentials.RegularUserEmail, UserConstants.Credentials.RegularUserPassword);
            }
        }
           
        private static IEnumerable<TestCaseData> InvalidInputTestCases
        {
            get
            {
                yield return new TestCaseData("", "", "電子郵件和密碼不能為空");
                yield return new TestCaseData("   ", "   ", "電子郵件和密碼不能為空白");
                yield return new TestCaseData("notanemail", "password123", "無效的電子郵件格式");

                string longEmail = "user@" + new string('a', 250) + ".com";
                yield return new TestCaseData(longEmail, "password123", "電子郵件長度超過限制");

                string longPassword = new('a', 257);
                yield return new TestCaseData("test@test.com", longPassword, "密碼長度超過限制");

                yield return new TestCaseData("<script>alert(1)</script>@test.com", "password123", "電子郵件包含不允許的字符");
                yield return new TestCaseData("test@test.com", "<script>alert(1)</script>", "密碼包含不允許的字符");
            }
        }

        [SetUp]
        public void Setup()
        {
            _client = TestSetup.GetClientWithoutToken();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        #region Login Tests
        [Category("Login")]
        [Test]
        [TestCaseSource(nameof(ValidUserCredentials))]
        public async Task Login_WithValidCredentials_ReturnsSuccessAndSetsCookie(string email, string password)
        {
            // Arrange
            var credentials = new LoginCredentialsDto
            {
                Email = email,
                Password = password
            };

            // Act
            var response = await _client.PostAsJsonAsync(TokenEndpoints.Login, credentials);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content!.AccessToken.Should().NotBeNullOrEmpty();

            // 驗證 Cookie
            var cookies = response.Headers.GetValues("Set-Cookie");
            cookies.Should().Contain(c => c.Contains("RefreshToken"));
            cookies.Should().Contain(c => c.Contains("httponly"));
            cookies.Should().Contain(c => c.Contains("secure"));
        }

        [Category("Login")]
        [Test]
        [TestCase("NotExistUser@example.com", "WrongPassword123!")]
        [TestCase(UserConstants.Credentials.RegularUserEmail, "WrongPassword123!")]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized(string email, string password)
        {
            // Arrange
            var credentials = new LoginCredentialsDto
            {
                Email = email,
                Password = password
            };

            // Act
            var response = await _client.PostAsJsonAsync(TokenEndpoints.Login, credentials);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Category("Login")]
        [TestCaseSource(nameof(InvalidInputTestCases))]
        public async Task Login_WithInvalidInput_ReturnsBadRequest(string email, string password, string testDescription)
        {
            // Arrange
            var credentials = new LoginCredentialsDto
            {
                Email = email,
                Password = password
            };

            // Act
            var response = await _client.PostAsJsonAsync(TokenEndpoints.Login, credentials);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeNullOrEmpty();
        }
        #endregion

        #region Refresh Tests
        [Test]
        [TestCaseSource(nameof(ValidUserCredentials))]
        public async Task Refresh_WithValidCookie_ReturnsNewTokens(string email, string password)
        {
            // Arrange
            var loginResponse = await LoginAndGetCookie(email, password);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResponse.accessToken);

            var cookieValue = $"RefreshToken={loginResponse.cookie.Value}";
            _client.DefaultRequestHeaders.Add("Cookie", cookieValue);

            // Act
            var response = await _client.PostAsync(TokenEndpoints.Refresh, null);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content!.AccessToken.Should().NotBeNullOrEmpty();

            var newCookies = response.Headers.GetValues("Set-Cookie");
            newCookies.Should().Contain(c => c.Contains("RefreshToken"));
        }

        [Test]
        public async Task Refresh_WithoutCookie_ReturnsUnauthorized()
        {
            // Arrange
            var loginResponse = await GetValidTokenResponse();
            var client = TestSetup.GetClientWithoutToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);

            // Act
            var response = await client.PostAsync(TokenEndpoints.Refresh, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("No refresh token provided");
        }

        [Test]
        public async Task Refresh_WithBlacklistedAccessToken_ReturnsUnauthorized()
        {
            // Arrange
            var loginResponse = await LoginAndGetCookie(
                UserConstants.Credentials.AdminEmail,
                UserConstants.Credentials.AdminPassword);

            var client = TestSetup.GetClientWithoutToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResponse.accessToken);

            // 將 token 加入黑名單
            var tokenService = TestSetup.GetService<ITokenService>();
            await tokenService.BlacklistAccessTokenAsync(loginResponse.accessToken);

            // 加入 Cookie
            var cookieValue = $"RefreshToken={loginResponse.cookie.Value}";
            client.DefaultRequestHeaders.Add("Cookie", cookieValue);

            // Act
            var response = await client.PostAsync(TokenEndpoints.Refresh, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Token has been blacklisted");
        }
        #endregion

        #region Revoke Tests
        [Test]
        [TestCaseSource(nameof(ValidUserCredentials))]
        public async Task Revoke_WithValidCookie_ReturnsSuccessAndBlacklistsToken(string email, string password)
        {
            // Arrange
            var loginResponse = await LoginAndGetCookie(email, password);

            // 使用相同的 client
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResponse.accessToken);

            // 加入 Cookie 到請求標頭
            var cookieValue = $"RefreshToken={loginResponse.cookie.Value}";
            _client.DefaultRequestHeaders.Add("Cookie", cookieValue);

            // Act
            var response = await _client.PostAsync(TokenEndpoints.Revoke, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // 驗證 Cookie 被清除
            var cookies = response.Headers.GetValues("Set-Cookie");
            cookies.Should().Contain(c => c.Contains("RefreshToken") && c.Contains("expires=Thu, 01 Jan 1970"));

            // 驗證 token 被加入黑名單
            var tokenService = TestSetup.GetService<ITokenService>();
            bool isBlacklisted = await TestSetup.ExecuteInScopeAsync(async provider =>
            {
                var tokenService = provider.GetRequiredService<ITokenService>();
                return await tokenService.IsAccessTokenBlacklistedAsync(loginResponse.accessToken);
            });
            isBlacklisted.Should().BeTrue();

            // 驗證使用已撤銷的 token 會返回 401
            var secondResponse = await _client.PostAsync(TokenEndpoints.Refresh, null);
            secondResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Revoke_WithoutCookie_ReturnsUnauthorized()
        {
            // Arrange
            var loginResponse = await GetValidTokenResponse();
            var client = TestSetup.GetClientWithoutToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);

            // Act
            var response = await client.PostAsync(TokenEndpoints.Revoke, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("No refresh token provided");
        }

        [Test]
        public async Task Revoke_WithBlacklistedAccessToken_ReturnsUnauthorized()
        {
            // Arrange
            var loginResponse = await LoginAndGetCookie(
                UserConstants.Credentials.AdminEmail,
                UserConstants.Credentials.AdminPassword);

            var client = TestSetup.GetClientWithoutToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResponse.accessToken);

            // 將 token 加入黑名單
            var tokenService = TestSetup.GetService<ITokenService>();
            await tokenService.BlacklistAccessTokenAsync(loginResponse.accessToken);

            // 加入 Cookie
            var cookieValue = $"RefreshToken={loginResponse.cookie.Value}";
            client.DefaultRequestHeaders.Add("Cookie", cookieValue);

            // Act
            var response = await client.PostAsync(TokenEndpoints.Revoke, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Token has been blacklisted");
        }
        #endregion

        private async Task<(string accessToken, Cookie cookie)> LoginAndGetCookie(string email, string password)
        {
            var credentials = new LoginCredentialsDto
            {
                Email = email,
                Password = password
            };

            var response = await _client.PostAsJsonAsync(TokenEndpoints.Login, credentials);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>()
                ?? throw new NullReferenceException("無回傳訊息");
            var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault()
                ?? throw new NullReferenceException("無法取得 Set-Cookie 標頭");
            var cookie = new Cookie("RefreshToken", setCookieHeader.Split(';').First().Split('=').Last());

            return (content.AccessToken, cookie);
        }


        private async Task<LoginResponse> GetValidTokenResponse()
        {
            var credentials = new LoginCredentialsDto
            {
                Email = UserConstants.Credentials.AdminEmail,
                Password = UserConstants.Credentials.AdminPassword
            };

            var response = await _client.PostAsJsonAsync(TokenEndpoints.Login, credentials);
            return await response.Content.ReadFromJsonAsync<LoginResponse>() ??
                throw new NullReferenceException("無回傳訊息");
        }
    }
}
