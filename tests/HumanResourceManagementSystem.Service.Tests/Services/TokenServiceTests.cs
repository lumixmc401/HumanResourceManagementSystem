using BuildingBlock.Security.Jwt;
using FluentAssertions;
using HumanResourceManagementSystem.Service.DTOs.Token;
using HumanResourceManagementSystem.Service.DTOs.User;
using HumanResourceManagementSystem.Service.Exceptions.Token;
using HumanResourceManagementSystem.Service.Implementations;
using HumanResourceManagementSystem.Service.Interfaces;
using Moq;
using NUnit.Framework;

namespace HumanResourceManagementSystem.Service.Tests.Services
{
    [TestFixture]
    public class TokenServiceTests
    {
        private Mock<ITokenCacheService> _mockTokenCache;
        private Mock<IJwtTokenGenerator> _mockJwtGenerator;
        private TokenService _service;

        [SetUp]
        public void Setup()
        {
            _mockTokenCache = new Mock<ITokenCacheService>();
            _mockJwtGenerator = new Mock<IJwtTokenGenerator>();
            _service = new TokenService(_mockTokenCache.Object, _mockJwtGenerator.Object);
        }

        [Test]
        public async Task BlacklistAccessTokenAsync_ShouldAddTokenToBlacklist()
        {
            // Arrange
            string accessToken = "test-access-token";
            _mockTokenCache.Setup(x => x.SetAsync(
                It.Is<string>(k => k.Contains(accessToken)),
                It.IsAny<string>(),
                It.IsAny<TimeSpan>()))
                .ReturnsAsync(true);

            // Act
            await _service.BlacklistAccessTokenAsync(accessToken);

            // Assert
            _mockTokenCache.Verify(x => x.SetAsync(
                It.Is<string>(k => k.Contains(accessToken)),
                "revoked",
                It.Is<TimeSpan>(t => t.TotalSeconds == 600)),
                Times.Once);
        }

        [Test]
        public async Task IsAccessTokenBlacklistedAsync_WhenTokenExists_ShouldReturnTrue()
        {
            // Arrange
            string accessToken = "test-access-token";
            _mockTokenCache.Setup(x => x.ExistsAsync(
                It.Is<string>(k => k.Contains(accessToken))))
                .ReturnsAsync(true);

            // Act
            var result = await _service.IsAccessTokenBlacklistedAsync(accessToken);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task GenerateTokensAsync_ShouldGenerateAndStoreTokens()
        {
            // Arrange
            var authResult = new AuthenticationResultDto
            {
                UserId = Guid.NewGuid(),
                UserName = "testuser",
                RoleIds = new[] { Guid.NewGuid() }
            };
            string deviceInfo = "test-device";
            string expectedAccessToken = "test-access-token";
            
            _mockJwtGenerator.Setup(x => x.GenerateJwtToken(
                authResult.UserId,
                authResult.UserName,
                authResult.RoleIds))
                .Returns(expectedAccessToken);

            _mockTokenCache.Setup(x => x.SetRefreshTokenAsync(
                It.IsAny<string>(),
                It.IsAny<TokenCacheDto>(),
                It.IsAny<TimeSpan>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.GenerateTokensAsync(authResult, deviceInfo);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be(expectedAccessToken);
            result.RefreshToken.Should().NotBeNullOrEmpty();
            
            _mockTokenCache.Verify(x => x.SetRefreshTokenAsync(
                It.IsAny<string>(),
                It.Is<TokenCacheDto>(d => 
                    d.UserId == authResult.UserId &&
                    d.UserName == authResult.UserName &&
                    d.DeviceInfo == deviceInfo),
                It.Is<TimeSpan>(t => t.TotalDays == 7)),
                Times.Once);
        }

        [Test]
        public async Task RefreshTokenAsync_WithValidToken_ShouldGenerateNewTokens()
        {
            // Arrange
            var request = new RefreshTokenRequest { RefreshToken = "valid-refresh-token" };
            string deviceInfo = "test-device";
            var cacheData = new TokenCacheDto
            {
                UserId = Guid.NewGuid(),
                UserName = "testuser",
                RoleIds = new[] { Guid.NewGuid() },
                DeviceInfo = deviceInfo
            };

            _mockTokenCache.Setup(x => x.GetRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync(cacheData);

            _mockJwtGenerator.Setup(x => x.GenerateJwtToken(
                cacheData.UserId,
                cacheData.UserName,
                cacheData.RoleIds))
                .Returns("new-access-token");

            // Act
            var result = await _service.RefreshTokenAsync(request, deviceInfo);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("new-access-token");
            result.RefreshToken.Should().NotBeNullOrEmpty();
            
            _mockTokenCache.Verify(x => x.RemoveRefreshTokenAsync(request.RefreshToken), Times.Once);
        }

        [Test]
        public async Task RefreshTokenAsync_WithInvalidToken_ShouldThrowException()
        {
            // Arrange
            var request = new RefreshTokenRequest { RefreshToken = "invalid-refresh-token" };
            string deviceInfo = "test-device";

            _mockTokenCache.Setup(x => x.GetRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync((TokenCacheDto)null);

            // Act & Assert
            await FluentActions.Invoking(() => 
                _service.RefreshTokenAsync(request, deviceInfo))
                .Should().ThrowAsync<InvalidRefreshTokenException>()
                .WithMessage("Invalid Refresh Token");
        }

        [Test]
        public async Task RevokeRefreshTokenAsync_WithValidToken_ShouldRemoveToken()
        {
            // Arrange
            var request = new RevokeRefreshTokenRequest { RefreshToken = "valid-refresh-token" };
            var cacheData = new TokenCacheDto
            {
                UserId = Guid.NewGuid(),
                UserName = "testuser",
                RoleIds = new[] { Guid.NewGuid() }
            };

            _mockTokenCache.Setup(x => x.GetRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync(cacheData);

            // Act
            await _service.RevokeRefreshTokenAsync(request);

            // Assert
            _mockTokenCache.Verify(x => x.RemoveRefreshTokenAsync(request.RefreshToken), Times.Once);
        }

        [Test]
        public async Task RevokeRefreshTokenAsync_WithInvalidToken_ShouldThrowException()
        {
            // Arrange
            var request = new RevokeRefreshTokenRequest { RefreshToken = "invalid-refresh-token" };

            _mockTokenCache.Setup(x => x.GetRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync((TokenCacheDto)null);

            // Act & Assert
            await FluentActions.Invoking(() => 
                _service.RevokeRefreshTokenAsync(request))
                .Should().ThrowAsync<InvalidRefreshTokenException>()
                .WithMessage("Invalid Refresh Token");
        }
    }
}
