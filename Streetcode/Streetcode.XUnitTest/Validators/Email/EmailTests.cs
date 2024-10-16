using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Email;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.Validators.Email;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Email;

public class EmailTests
{
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly SendEmailCommandValidator validator;

    public EmailTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.validator = new SendEmailCommandValidator(this.mockNamesLocalizer, this.mockValidationLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenEmailIsValid()
    {
        // Arrange
        var email = this.GetValidEmailDto();

        // Act
        var result = this.validator.Validate(new SendEmailCommand(email));

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenFromIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["Email"]];
        var email = this.GetValidEmailDto();
        email.From = string.Empty;

        // Act
        var result = this.validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.From)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenFromLengthIsOutOfRange()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Email"], SendEmailCommandValidator.EmailMaxLength];
        var email = this.GetValidEmailDto();
        email.From = new string('e', SendEmailCommandValidator.EmailMaxLength + 2);

        // Act
        var result = this.validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.From)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenEmailFormatIsInvalid()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["EmailAddressFormat"];
        var email = this.GetValidEmailDto();
        email.From = "invalid////";

        // Act
        var result = this.validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.From)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTokenIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["CaptchaToken"]];
        var email = this.GetValidEmailDto();
        email.Token = string.Empty;

        // Act
        var result = this.validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.Token)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenContentLengthIsOutOfRange()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["LengthMustBeInRange", this.mockNamesLocalizer["Content"], SendEmailCommandValidator.ContentMinLength, SendEmailCommandValidator.ContentMaxLength];
        var email = this.GetValidEmailDto();
        email.Content = new string('c', SendEmailCommandValidator.ContentMaxLength + 2);

        // Act
        var result = this.validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.Content)
            .WithErrorMessage(expectedError);
    }

    private EmailDTO GetValidEmailDto()
    {
        return new EmailDTO()
        {
            From = "test@gmail.com",
            Token = "token",
            Content = "Test Content",
        };
    }
}