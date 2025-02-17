using BuildingBlock.Core.Share.Helpers;
using FluentAssertions;
using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource;
using HumanResourceManagementSystem.Service.DTOs.Role;
using HumanResourceManagementSystem.Service.DTOs.User;
using HumanResourceManagementSystem.Service.DTOs.UserClaim;
using HumanResourceManagementSystem.Service.Exceptions.User;
using HumanResourceManagementSystem.Service.Implementations;
using Moq;
using System.Security.Claims;

namespace ServiceTest
{
    public class UserServiceTests
    {
        private Mock<IHumanResourceUnitOfWork> _mockUnitOfWork;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IHumanResourceUnitOfWork>();
            _userService = new UserService(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task CreateUserAsync_ShouldCreateUser()
        {
            // Arrange
            var userDto = new CreateUserDto
            {
                Email = "test@example.com",
                Password = "password",
                Roles = [new RoleDto { Id = Guid.NewGuid(), Name = "Admin" }],
                Claims = [new UserClaimDto { Type = "ClaimType", Value = "ClaimValue" }]
            };

            _mockUnitOfWork.Setup(u => u.Users.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.FromResult(1));

            // Act
            await _userService.CreateUserAsync(userDto);

            // Assert
            _mockUnitOfWork.Verify(u => u.Users.AddAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UpdateUserDto
            {
                Id = userId,
                Email = "updated@example.com",
                Roles = [new RoleDto { Id = Guid.NewGuid(), Name = "User" }],
                Claims = [new UserClaimDto { Type = "UpdatedClaimType", Value = "UpdatedClaimValue" }]
            };

            var user = new User { Id = userId, Email = "test@example.com" };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(u => u.Users.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.FromResult(1));

            // Act
            await _userService.UpdateUserAsync(userDto);

            // Assert
            _mockUnitOfWork.Verify(u => u.Users.UpdateAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Email = "test@example.com" };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(u => u.Users.RemoveAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.FromResult(1));

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _mockUnitOfWork.Verify(u => u.Users.RemoveAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdatePasswordAsync_ShouldUpdatePassword()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newPassword = "newPassword";
            var user = new User { Id = userId, Email = "test@example.com" };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(u => u.Users.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.FromResult(1));

            // Act
            await _userService.UpdatePasswordAsync(new UpdateUserPasswordDto 
            {
                Id = userId,
                NewPassword = newPassword,
            });

            // Assert
            _mockUnitOfWork.Verify(u => u.Users.UpdateAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Email = "test@example.com" };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            result.Should().Be(user);
        }

        [Test]
        public void GetUserByIdAsync_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            Func<Task> act = () => _userService.GetUserByIdAsync(userId);

            // Assert
            act.Should().ThrowAsync<UserNotFoundException>();
        }
        
        [Test]
        public async Task VerifyUser_WithValidCredentials_ShouldReturnVerifiedResponse()
        {
            // Arrange
            string email = "test@example.com";
            string password = "password";
            byte[] salt = [1,2,3];
            Guid userId = Guid.NewGuid();
            string userName = "Test User";
            Guid roleId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Email = email,
                PasswordHash = PasswordHelper.HashPassword(password, salt),
                Salt = Convert.ToBase64String(salt),
                UserClaims = new List<UserClaim>
                {
                    new() { ClaimType = ClaimTypes.Name, ClaimValue = userName }
                },
                UserRoles = new List<UserRole>
                {
                    new() { RoleId = roleId }
                }
            };

            _mockUnitOfWork.Setup(u => u.Users.GetUserByEmailAsync(email))
                .ReturnsAsync(user);

            var dto = new LoginCredentialsDto
            {
                Email = email,
                Password = password
            };

            // Act
            var result = await _userService.AuthenticateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.UserName.Should().Be(userName);
            result.RoleIds.Should().Contain(roleId);
        }

        [Test]
        public async Task VerifyUser_WithInvalidEmail_ShouldThrowUserUnauthorizedException()
        {
            // Arrange
            var dto = new LoginCredentialsDto
            {
                Email = "nonexistent@example.com",
                Password = "password"
            };

            _mockUnitOfWork.Setup(u => u.Users.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            // Act
            Func<Task> act = () => _userService.AuthenticateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<UserUnauthorizedException>();
        }

        [Test]
        public async Task UpdateUserAsync_WithInvalidId_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userDto = new UpdateUserDto
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Roles = new List<RoleDto>(),
                Claims = new List<UserClaimDto>()
            };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(userDto.Id))
                .ReturnsAsync((User?)null);

            // Act
            Func<Task> act = () => _userService.UpdateUserAsync(userDto);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Test]
        public async Task UpdatePasswordAsync_WithInvalidId_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var dto = new UpdateUserPasswordDto
            {
                Id = Guid.NewGuid(),
                NewPassword = "newPassword"
            };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(dto.Id))
                .ReturnsAsync((User?)null);

            // Act
            Func<Task> act = () => _userService.UpdatePasswordAsync(dto);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Test]
        public async Task DeleteUserAsync_WithInvalidId_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act
            Func<Task> act = () => _userService.DeleteUserAsync(userId);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }
    }
}
