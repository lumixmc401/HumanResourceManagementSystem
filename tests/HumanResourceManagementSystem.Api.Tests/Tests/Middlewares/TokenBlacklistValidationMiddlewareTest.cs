using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using HumanResourceManagementSystem.Api.Models.Response;
using HumanResourceManagementSystem.Api.Tests.Constants.Api;
using HumanResourceManagementSystem.Api.Tests.Constants.Data;
using HumanResourceManagementSystem.Api.Tests.Factories;
using HumanResourceManagementSystem.Service.DTOs.User;
using HumanResourceManagementSystem.Service.Interfaces;

namespace HumanResourceManagementSystem.Api.Tests.Tests.Controllers
{
    public class TokenBlacklistValidationMiddlewareTest
    {
        private const string TestEndPoint = UserEndPoints.Profile;

        [Test]
        public async Task Middleware_WithBlacklistedToken_BlocksRequest()
        {
            // Arrange
            using var client = await TestClientFactory.CreateAuthenticatedClient(
                UserConstants.Credentials.AdminEmail,
                UserConstants.Credentials.AdminPassword);

            // 取得 token 後加入黑名單
            var token = client.DefaultRequestHeaders.Authorization?.Parameter;
            token.Should().NotBeNullOrEmpty("Token should be present in authenticated client");

            var tokenService = TestSetup.GetService<ITokenService>();
            await tokenService.BlacklistAccessTokenAsync(token!);

            // Act
            var response = await client.GetAsync(TestEndPoint);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Token has been blacklisted");
        }

        [Test]
        public async Task Middleware_WithValidToken_PassesThrough()
        {
            // Arrange
            using var client = await TestClientFactory.CreateAuthenticatedClient(
                UserConstants.Credentials.AdminEmail,
                UserConstants.Credentials.AdminPassword);

            // Act
            var response = await client.GetAsync(TestEndPoint);

            // Assert
            response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Middleware_WithoutToken_PassesThrough()
        {
            // Arrange
            using var client = TestClientFactory.CreateClient();

            // Act
            var response = await client.GetAsync(TestEndPoint);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotContain("Token has been blacklisted");
        }
    }
}

