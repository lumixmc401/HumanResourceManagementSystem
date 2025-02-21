using FluentAssertions;
using HumanResourceManagementSystem.Service.DTOs.Token;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;

namespace HumanResourceManagementSystem.Service.Tests.Services
{
    public class RedisTokenCacheServiceTests
    {
        private Mock<IConnectionMultiplexer> _mockRedis;
        private Mock<IDatabase> _mockDatabase;
        private RedisTokenCacheService _service;
        private readonly string _instanceName = "test-instance";

        [SetUp]
        public void Setup()
        {
            _mockRedis = new Mock<IConnectionMultiplexer>();
            _mockDatabase = new Mock<IDatabase>();
            var redisSettings = Options.Create(new RedisSettings { InstanceName = _instanceName });

            _mockRedis.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_mockDatabase.Object);

            _service = new RedisTokenCacheService(_mockRedis.Object, redisSettings);
        }

        [Test]
        public async Task SetRefreshTokenAsync_ShouldSetValueInRedis()
        {
            // Arrange
            string token = "test-token";
            var cacheData = new TokenCacheDto
            {
                UserId = Guid.NewGuid(),
                UserName = "Test User",
                RoleIds = new[] { Guid.NewGuid() },
                Token = "jwt-token",
                CreatedAt = DateTime.UtcNow,
                DeviceInfo = "test-device"
            };
            var expiry = TimeSpan.FromMinutes(30);

            _mockDatabase.Setup(x => x.StringSetAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(token)),
                It.IsAny<RedisValue>(),
                expiry,
                false,
                When.Always,
                CommandFlags.None))
                .ReturnsAsync(true);

            // Act
            var result = await _service.SetRefreshTokenAsync(token, cacheData, expiry);

            // Assert
            result.Should().BeTrue();
            _mockDatabase.Verify(x => x.StringSetAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(token)),
                It.IsAny<RedisValue>(),
                expiry,
                false,
                When.Always,
                CommandFlags.None), 
                Times.Once);
        }

        [Test]
        public async Task GetRefreshTokenAsync_WhenTokenExists_ShouldReturnCacheData()
        {
            // Arrange
            string token = "test-token";
            var cacheData = new TokenCacheDto
            {
                UserId = Guid.NewGuid(),
                UserName = "Test User",
                RoleIds = new[] { Guid.NewGuid() },
                Token = "jwt-token",
                CreatedAt = DateTime.UtcNow,
                DeviceInfo = "test-device"
            };

            _mockDatabase.Setup(x => x.StringGetAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(token)),
                CommandFlags.None))
                .ReturnsAsync(System.Text.Json.JsonSerializer.Serialize(cacheData));

            // Act
            var result = await _service.GetRefreshTokenAsync(token);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(cacheData);
        }

        [Test]
        public async Task GetRefreshTokenAsync_WhenTokenNotExists_ShouldReturnNull()
        {
            // Arrange
            string token = "non-existent-token";

            _mockDatabase.Setup(x => x.StringGetAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(token)),
                CommandFlags.None))
                .ReturnsAsync(RedisValue.Null);

            // Act
            var result = await _service.GetRefreshTokenAsync(token);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task RemoveRefreshTokenAsync_ShouldDeleteKey()
        {
            // Arrange
            string token = "test-token";

            _mockDatabase.Setup(x => x.KeyDeleteAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(token)),
                CommandFlags.None))
                .ReturnsAsync(true);

            // Act
            var result = await _service.RemoveRefreshTokenAsync(token);

            // Assert
            result.Should().BeTrue();
            _mockDatabase.Verify(x => x.KeyDeleteAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(token)),
                CommandFlags.None),
                Times.Once);
        }

        [Test]
        public async Task IsRefreshTokenValidAsync_WhenTokenExists_ShouldReturnTrue()
        {
            // Arrange
            string token = "test-token";

            _mockDatabase.Setup(x => x.KeyExistsAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(token)),
                CommandFlags.None))
                .ReturnsAsync(true);

            // Act
            var result = await _service.IsRefreshTokenValidAsync(token);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task IsRefreshTokenValidAsync_WhenTokenNotExists_ShouldReturnFalse()
        {
            // Arrange
            string token = "non-existent-token";

            _mockDatabase.Setup(x => x.KeyExistsAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(token)),
                CommandFlags.None))
                .ReturnsAsync(false);

            // Act
            var result = await _service.IsRefreshTokenValidAsync(token);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task SetAsync_ShouldSetValueInRedis()
        {
            // Arrange
            string key = "test-key";
            string value = "test-value";
            var expiry = TimeSpan.FromMinutes(30);

            _mockDatabase.Setup(x => x.StringSetAsync(
                key,
                value,
                expiry,
                false,
                When.Always,
                CommandFlags.None))
                .ReturnsAsync(true);

            // Act
            var result = await _service.SetAsync(key, value, expiry);

            // Assert
            result.Should().BeTrue();
            _mockDatabase.Verify(x => x.StringSetAsync(
                key,
                value,
                expiry,
                false,
                When.Always,
                CommandFlags.None),
                Times.Once);
        }

        [Test]
        public async Task ExistsAsync_ShouldCheckKeyExistence()
        {
            // Arrange
            string key = "test-key";

            _mockDatabase.Setup(x => x.KeyExistsAsync(
                key,
                CommandFlags.None))
                .ReturnsAsync(true);

            // Act
            var result = await _service.ExistsAsync(key);

            // Assert
            result.Should().BeTrue();
            _mockDatabase.Verify(x => x.KeyExistsAsync(
                key,
                CommandFlags.None),
                Times.Once);
        }

        [Test]
        public async Task RemoveAsync_ShouldDeleteKey()
        {
            // Arrange
            string key = "test-key";

            _mockDatabase.Setup(x => x.KeyDeleteAsync(
                key,
                CommandFlags.None))
                .ReturnsAsync(true);

            // Act
            var result = await _service.RemoveAsync(key);

            // Assert
            result.Should().BeTrue();
            _mockDatabase.Verify(x => x.KeyDeleteAsync(
                key,
                CommandFlags.None),
                Times.Once);
        }
    }
}
