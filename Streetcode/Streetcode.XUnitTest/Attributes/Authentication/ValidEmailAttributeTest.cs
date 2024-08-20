using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Attributes.Authentication;
using Xunit;

namespace Streetcode.XUnitTest.Attributes.Authentication
{
    public class ValidEmailAttributeTest
    {
        private readonly ValidEmailAttribute _validEmailAttribute;

        public ValidEmailAttributeTest()
        {
            this._validEmailAttribute = new ValidEmailAttribute();
        }

        [Fact]
        public void ShouldReturnCorrectFailMessage_InputParameterIsNull()
        {
            // Arrange.
            string expectedErrorMessage = "Input parameter cannot be null";
            object? nullParam = null;

            // Act.
            var result = this._validEmailAttribute.GetValidationResult(nullParam, this.GetValidationContext());

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
            var result = this._validEmailAttribute.GetValidationResult(nullParam, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Theory]
        [InlineData("test@@test.com")]
        [InlineData("@.com")]
        [InlineData("test@test.ru")]
        [InlineData("asda @test.com")]
        [InlineData("test@.")]
        [InlineData("test@com")]
        public void ShouldReturnCorrectFailMessage_InvalidEmailFormat(string email)
        {
            // Arrange.
            string expectedErrorMessage = "Incorrect email address format";

            // Act.
            var result = this._validEmailAttribute.GetValidationResult(email, this.GetValidationContext());

            // Assert.
            Assert.Equal(expectedErrorMessage, result!.ErrorMessage);
        }

        [Theory]
        [InlineData("test@test.com")]
        [InlineData("___test@test.com")]
        [InlineData("--___--@test.com")]
        [InlineData("QQQQQQQ@QQQQ.com")]
        [InlineData("....@...com")]
        public void ShouldReturnSuccess_ValidEmail(string email)
        {
            // Arrange.

            // Act.
            var result = this._validEmailAttribute.GetValidationResult(email, this.GetValidationContext());

            // Assert.
            Assert.Equal(ValidationResult.Success, result);
        }

        private ValidationContext GetValidationContext()
        {
            return new ValidationContext(new object());
        }
    }
}
