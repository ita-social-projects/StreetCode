using Streetcode.BLL.Attributes.Authentication;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.MediatR.Authentication.Login;
using Xunit;
using System.ComponentModel.DataAnnotations;

namespace Streetcode.XUnitTest.Attributes.Authentication
{
    public class StrongPasswordAttributeTest
    {
        private readonly StrongPasswordAttribute _strongPasswordAttribute;

        public StrongPasswordAttributeTest()
        {
            this._strongPasswordAttribute = new StrongPasswordAttribute();
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_InputParameterIsNull()
        {
            // Arrange.
            string expectedErrorMessage = "Input parameter cannot be null";
            object? nullParam = null;

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(nullParam, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result !.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_InputParameterIsNotString()
        {
            // Arrange.
            string expectedErrorMessage = "Attribute cannot be applied to non-string property";
            object? nullParam = new object();

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(nullParam, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordContainsWhitespaces()
        {
            // Arrange.
            string expectedErrorMessage = "Password cannot contain whitespaces";
            string password = "qwer ty";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordHasNoDigits()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain digit";
            string password = "qwerty";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordHasNoNonAlphaNumericCharacters()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain non-alphanumeric symbol";
            string password = "qwerty1";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordContainsPersentCharacter()
        {
            // Arrange.
            string expectedErrorMessage = "Password cannot contain '%'";
            string password = "qwerty1@%";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordHasNoUppercaseLetter()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain UPPERCASE letter";
            string password = "qwerty1@";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordHasNoLowercaseLetter()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain lowercase letter";
            string password = "QWERTY1@";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Theory]
        [InlineData("Qwerty123@#")]
        [InlineData("@1234540956pW")]
        [InlineData("*(&#@^#qW1")]
        [InlineData("____Q....w,,,,1")]
        public async Task ShouldReturnSuccess_ValidPassword(string password)
        {
            // Arrange.

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(ValidationResult.Success, result);
        }

        private ValidationContext GetValidationContext()
        {
            return new ValidationContext(new object());
        }
    }
}
