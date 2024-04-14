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
        public async Task ShouldReturnCorrectFailMessage_PasswordLengthLessThan14()
        {
            // Arrange.
            string expectedErrorMessage = "Password minimum length is 14";
            string password = "qwer";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordContainsWhitespaces()
        {
            // Arrange.
            string expectedErrorMessage = "Password cannot contain whitespaces";
            string password = "qwer tyqwertyqwerty";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordHasNoDigits()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain at least one digit";
            string password = "qwertyqweqwerty";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordHasNoNonAlphaNumericCharacters()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain at least one non-alphanumeric symbol";
            string password = "qwerty1qwertyqwe";

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
            string password = "qwerty1@%qwertyqwe";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordHasNoUppercaseLetter()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain at least one UPPERCASE letter";
            string password = "qwerty1@qwertyqwe";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReturnCorrectFailMessage_PasswordHasNoLowercaseLetter()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain at least one lowercase letter";
            string password = "QWERTY1@QWERTYQWE";

            // Act.
            var result = this._strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Theory]
        [InlineData("Qwerty123@#asdfg")]
        [InlineData("@1234540956pWWWWWW")]
        [InlineData("*(&#@^#qW1@&@($&@#_!@")]
        [InlineData("____Q....w,,,,1....,")]
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
