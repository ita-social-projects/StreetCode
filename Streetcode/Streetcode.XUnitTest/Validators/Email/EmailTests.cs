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
        mockValidationLocalizer = new MockFailedToValidateLocalizer();
        mockNamesLocalizer = new MockFieldNamesLocalizer();
        validator = new SendEmailCommandValidator(mockNamesLocalizer, mockValidationLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenEmailIsValid()
    {
        // Arrange
        var email = GetValidEmailDto();

        // Act
        var result = validator.Validate(new SendEmailCommand(email));

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenFromIsEmpty()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["CannotBeEmpty", mockNamesLocalizer["Email"]];
        var email = GetValidEmailDto();
        email.From = string.Empty;

        // Act
        var result = validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.From)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenFromLengthIsOutOfRange()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MaxLength", mockNamesLocalizer["Email"], SendEmailCommandValidator.EmailMaxLength];
        var email = GetValidEmailDto();
        email.From = new string('e', SendEmailCommandValidator.EmailMaxLength + 2);

        // Act
        var result = validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.From)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenEmailFormatIsInvalid()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["EmailAddressFormat"];
        var email = GetValidEmailDto();
        email.From = "invalid////";

        // Act
        var result = validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.From)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTokenIsEmpty()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["CannotBeEmpty", mockNamesLocalizer["CaptchaToken"]];
        var email = GetValidEmailDto();
        email.Token = string.Empty;

        // Act
        var result = validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.Token)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenContentLengthIsOutOfRange()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["LengthMustBeInRange", mockNamesLocalizer["Content"], SendEmailCommandValidator.ContentMinLength, SendEmailCommandValidator.ContentMaxLength];
        var email = GetValidEmailDto();
        email.Content = new string('c', SendEmailCommandValidator.ContentMaxLength + 2);

        // Act
        var result = validator.TestValidate(new SendEmailCommand(email));

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