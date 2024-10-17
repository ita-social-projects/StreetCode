using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Attributes.Authentication;
using Xunit;

namespace Streetcode.XUnitTest.Attributes.Authentication
{
    public class StrongPasswordAttributeTest
    {
        private readonly StrongPasswordAttribute strongPasswordAttribute;

        public StrongPasswordAttributeTest()
        {
            this.strongPasswordAttribute = new StrongPasswordAttribute();
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_InputParameterIsNull()
        {
            // Arrange.
            string expectedErrorMessage = "Input parameter cannot be null";
            object? nullParam = null;

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(nullParam, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result !.ErrorMessage);
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_InputParameterIsNotString()
        {
            // Arrange.
            string expectedErrorMessage = "Attribute cannot be applied to non-string property";
            object? nullParam = new object();

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(nullParam, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_PasswordLengthLessThan14()
        {
            // Arrange.
            string expectedErrorMessage = "Password minimum length is 14";
            string password = "qwer";

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_PasswordContainsWhitespaces()
        {
            // Arrange.
            string expectedErrorMessage = "Password cannot contain whitespaces";
            string password = "qwer tyqwertyqwerty";

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_PasswordHasNoDigits()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain at least one digit";
            string password = "qwertyqweqwerty";

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_PasswordHasNoNonAlphaNumericCharacters()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain at least one non-alphanumeric symbol";
            string password = "qwerty1qwertyqwe";

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_PasswordContainsPersentCharacter()
        {
            // Arrange.
            string expectedErrorMessage = "Password cannot contain '%'";
            string password = "qwerty1@%qwertyqwe";

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_PasswordHasNoUppercaseLetter()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain at least one UPPERCASE letter";
            string password = "qwerty1@qwertyqwe";

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_PasswordHasNoLowercaseLetter()
        {
            // Arrange.
            string expectedErrorMessage = "Password must contain at least one lowercase letter";
            string password = "QWERTY1@QWERTYQWE";

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Theory]
        [InlineData("Qwerty123@#asdfg")]
        [InlineData("@1234540956pWWWWWW")]
        [InlineData("*(&#@^#qW1@&@($&@#_!@")]
        [InlineData("____Q....w,,,,1....,")]
        public void ShouldReturnSuccess_ValidPassword(string password)
        {
            // Arrange.

            // Act.
            var result = this.strongPasswordAttribute.GetValidationResult(password, this.GetValidationContext());

            // Assert.
            Assert.Equal(ValidationResult.Success, result);
        }

        private ValidationContext GetValidationContext()
        {
            return new ValidationContext(new object());
        }
    }
}
