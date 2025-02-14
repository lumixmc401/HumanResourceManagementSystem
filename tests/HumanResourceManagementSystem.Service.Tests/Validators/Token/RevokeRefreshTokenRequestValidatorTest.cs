using FluentValidation.TestHelper;
using HumanResourceManagementSystem.Service.DTOs.Token;
using HumanResourceManagementSystem.Service.Validators.Token;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Tests.Validators.Token
{
    [TestFixture]
    public class RevokeRefreshTokenRequestValidatorTest
    {
        private RevokeRefreshTokenRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new RevokeRefreshTokenRequestValidator();
        }

        [Test]
        public async Task ValidateRefreshToken_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RevokeRefreshTokenRequest { RefreshToken = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
                 .WithErrorMessage("��s�O�P���ର��");
        }

        [Test]
        public async Task ValidateRefreshToken_WhenNull_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RevokeRefreshTokenRequest { RefreshToken = null! };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
                 .WithErrorMessage("��s�O�P���ର��");
        }

        [Test]
        public async Task ValidateRefreshToken_WhenExceedsMaxLength_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RevokeRefreshTokenRequest 
            { 
                RefreshToken = new string('x', 501) // �إߪ��׬�501���r��
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
                 .WithErrorMessage("��s�O�P���פ���W�L 500 �Ӧr��");
        }

        [Test]
        public async Task ValidateRefreshToken_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new RevokeRefreshTokenRequest 
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
