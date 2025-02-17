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
using HumanResourceManagementSystem.Api.Tests.Factory;

namespace HumanResourceManagementSystem.Api.Tests.Tests.Controllers
{
    public class TokenControllerTest
    {
        private static IEnumerable<TestCaseData> ValidUserCredentials
        {
            get
            {
                yield return new TestCaseData(UserConstants.Credentials.AdminEmail, UserConstants.Credentials.AdminPassword);
                yield return new TestCaseData(UserConstants.Credentials.RegularUserEmail, UserConstants.Credentials.RegularUserPassword);
            }
        }

        private static IEnumerable<TestCaseData> InvalidUserCredentials
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

        #region Login Tests
        [Test]
        [TestCaseSource(nameof(ValidUserCredentials))]
        public async Task Login_WithValidCredentials_ReturnsSuccessAndSetsCookie(string email, string password)
        {
            // Arrange
            using var client = TestClientFactory.CreateClient();
            var credentials = new LoginCredentialsDto
            {
                Email = email,
                Password = password
            };

            // Act
            var response = await client.PostAsJsonAsync(TokenEndpoints.Login, credentials);
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

        [Test]
        [TestCase("NotExistUser@example.com", "WrongPassword123!")]
        [TestCase(UserConstants.Credentials.RegularUserEmail, "WrongPassword123!")]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized(string email, string password)
        {
            // Arrange
            using var client = TestClientFactory.CreateClient();
            var credentials = new LoginCredentialsDto
            {
                Email = email,
                Password = password
            };

            // Act
            var response = await client.PostAsJsonAsync(TokenEndpoints.Login, credentials);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [TestCaseSource(nameof(InvalidUserCredentials))]
        public async Task Login_WithInvalidInput_ReturnsBadRequest(string email, string password, string testDescription)
        {
            // Arrange
            using var client = TestClientFactory.CreateClient();
            var credentials = new LoginCredentialsDto
            {
                Email = email,
                Password = password
            };

            // Act
            var response = await client.PostAsJsonAsync(TokenEndpoints.Login, credentials);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Login_WithNullDto_ReturnsBadRequest()
        {
            // Arrange
            using var client = TestClientFactory.CreateClient();
            LoginCredentialsDto? dto = null;

            // Act
            var response = await client.PostAsJsonAsync(TokenEndpoints.Login, dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Refresh Tests
        [Test]
        [TestCaseSource(nameof(ValidUserCredentials))]
        public async Task Refresh_WithValidRefreshToken_ReturnsNewTokens(string email, string password)
        {
            // Arrange
            using var client = await TestClientFactory.CreateAuthenticatedClient(email, password);

            // Act
            var response = await client.PostAsync(TokenEndpoints.Refresh, null);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content!.AccessToken.Should().NotBeNullOrEmpty();

            var newCookies = response.Headers.GetValues("Set-Cookie");
            newCookies.Should().Contain(c => c.Contains("RefreshToken"));
        }

        [Test]
        public async Task Refresh_WithoutRefreshToken_ReturnsUnauthorized()
        {
            // Arrange
            using var client = await TestClientFactory.CreateAuthenticatedClient(
                UserConstants.Credentials.AdminEmail,
                UserConstants.Credentials.AdminPassword);

            // 清除 Cookie
            client.DefaultRequestHeaders.Remove("Cookie");

            // Act
            var response = await client.PostAsync(TokenEndpoints.Refresh, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("No Refresh Token Provide");
        }

        [Test]
        public async Task Refresh_WithInvalidRefreshToken_ReturnsUnauthorized()
        {
            // Arrange
            using var client = await TestClientFactory.CreateAuthenticatedClient(
                UserConstants.Credentials.AdminEmail,
                UserConstants.Credentials.AdminPassword);

            // 替換為無效的 RefreshToken
            client.DefaultRequestHeaders.Remove("Cookie");
            client.DefaultRequestHeaders.Add("Cookie", "RefreshToken=invalid_refresh_token");

            // Act
            var response = await client.PostAsync(TokenEndpoints.Refresh, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Invalid Refresh Token");
        }
        #endregion

        #region Revoke Tests
        [Test]
        [TestCaseSource(nameof(ValidUserCredentials))]
        public async Task Revoke_WithValidRefreshToken_ReturnsSuccess(string email, string password)
        {
            // Arrange
            using var client = await TestClientFactory.CreateAuthenticatedClient(email, password);

            // Act
            var response = await client.PostAsync(TokenEndpoints.Revoke, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // 驗證 Cookie 被清除
            var cookies = response.Headers.GetValues("Set-Cookie");
            cookies.Should().Contain(c => c.Contains("RefreshToken") && c.Contains("expires=Thu, 01 Jan 1970"));

            // 驗證使用已撤銷的 token 會返回 401
            var secondResponse = await client.PostAsync(TokenEndpoints.Refresh, null);
            secondResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Revoke_WithoutRefreshToken_ReturnsUnauthorized()
        {
            // Arrange
            using var client = await TestClientFactory.CreateAuthenticatedClient(
                UserConstants.Credentials.AdminEmail,
                UserConstants.Credentials.AdminPassword);

            // 清除 Cookie
            client.DefaultRequestHeaders.Remove("Cookie");

            // Act
            var response = await client.PostAsync(TokenEndpoints.Revoke, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            string content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("No Refresh Token Provide");
        }

        [Test]
        public async Task Revoke_WithInvalidRefreshToken_ReturnsUnauthorized()
        {
            // Arrange
            using var client = await TestClientFactory.CreateAuthenticatedClient(
                UserConstants.Credentials.AdminEmail,
                UserConstants.Credentials.AdminPassword);

            // 替換為無效的 RefreshToken
            client.DefaultRequestHeaders.Remove("Cookie");
            client.DefaultRequestHeaders.Add("Cookie", "RefreshToken=invalid_refresh_token");

            // Act
            var response = await client.PostAsync(TokenEndpoints.Revoke, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Invalid Refresh Token");
        }
        #endregion
    }
}
