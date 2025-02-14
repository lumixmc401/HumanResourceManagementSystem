using FluentValidation.TestHelper;
using HumanResourceManagementSystem.Service.DTOs.Token;
using HumanResourceManagementSystem.Service.Validators.Token;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Tests.Validators.Token
{
    [TestFixture]
    public class RefreshTokenRequestValidatorTest
    {
        private RefreshTokenRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new RefreshTokenRequestValidator();
        }

        [Test]
        public async Task ValidateRefreshToken_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RefreshTokenRequest { RefreshToken = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
                 .WithErrorMessage("刷新令牌不能為空");
        }

        [Test]
        public async Task ValidateRefreshToken_WhenNull_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RefreshTokenRequest { RefreshToken = null! };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
                 .WithErrorMessage("刷新令牌不能為空");
        }

        [Test]
        public async Task ValidateRefreshToken_WhenExceedsMaxLength_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RefreshTokenRequest
            { 
                RefreshToken = new string('x', 501) // 建立長度為501的字串
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
                 .WithErrorMessage("刷新令牌長度不能超過 500 個字符");
        }

        [Test]
        public async Task ValidateRefreshToken_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new RefreshTokenRequest
            { 
                RefreshToken = "valid-refresh-token" 
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
