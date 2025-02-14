using FluentValidation.TestHelper;
using HumanResourceManagementSystem.Service.DTOs.User;
using HumanResourceManagementSystem.Service.Validators.Token;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Tests.Validators.Token
{
    [TestFixture]
    public class LoginCredentialsDtoValidatorTest
    {
        private LoginCredentialsDtoValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new LoginCredentialsDtoValidator();
        }

        [TestCase("")]
        [TestCase("invalid-email")]
        [TestCase("test@")]
        [TestCase("@domain.com")]
        public async Task ValidateEmail_WhenInvalidFormat_ShouldHaveValidationError(string email)
        {
            // Arrange
            var dto = new LoginCredentialsDto { Email = email, Password = "validPassword123" };

            // Act
            var result = await _validator.TestValidateAsync(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                 .WithErrorMessage("Email 格式不正確");
        }

        [Test]
        public async Task ValidateEmail_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longEmail = new string('a', 248) + "@test.com";
            var dto = new LoginCredentialsDto { Email = longEmail, Password = "validPassword123" };

            // Act
            var result = await _validator.TestValidateAsync(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                 .WithErrorMessage("Email 長度不能超過 256 個字元");
        }

        [Test]
        public async Task ValidatePassword_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var dto = new LoginCredentialsDto { Email = "test@example.com", Password = "" };

            // Act
            var result = await _validator.TestValidateAsync(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                 .WithErrorMessage("密碼不能為空");
        }

        [Test]
        public async Task ValidatePassword_WhenTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var dto = new LoginCredentialsDto { Email = "test@example.com", Password = "123" };

            // Act
            var result = await _validator.TestValidateAsync(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                 .WithErrorMessage("密碼至少需要 8 個字元");
        }

        [Test]
        public async Task ValidatePassword_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longPassword = new string('a', 257);
            var dto = new LoginCredentialsDto { Email = "test@example.com", Password = longPassword };

            // Act
            var result = await _validator.TestValidateAsync(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                 .WithErrorMessage("密碼長度不能超過 256 個字元");
        }

        [Test]
        public async Task Validate_WhenAllValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var dto = new LoginCredentialsDto
            {
                Email = "test@example.com",
                Password = "validPassword123"
            };

            // Act
            var result = await _validator.TestValidateAsync(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        // 新增 XSS 防範相關的測試
        [TestCase("<script>alert('xss')</script>@example.com")]
        [TestCase("test@example.com<script>")]
        public async Task ValidateEmail_WhenContainsXss_ShouldHaveValidationError(string email)
        {
            // Arrange
            var dto = new LoginCredentialsDto { Email = email, Password = "validPassword123" };

            // Act
            var result = await _validator.TestValidateAsync(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                 .WithErrorMessage("Email 包含不安全的字元");
        }

        [TestCase("<script>alert('xss')</script>")]
        [TestCase("password123<script>")]
        [TestCase("<img src=x onerror=alert('xss')>")]
        [TestCase("<a href='JAVASCRIPT:alert(1)'>")]
        public async Task ValidatePassword_WhenContainsXss_ShouldHaveValidationError(string password)
        {
            // Arrange
            var dto = new LoginCredentialsDto { Email = "test@example.com", Password = password };

            // Act
            var result = await _validator.TestValidateAsync(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                 .WithErrorMessage("Password 包含不安全的字元");
        }
    }
}
