using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Email;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.Validators.Email;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Email;

public class EmailTests
{
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly SendEmailCommandValidator _validator;

    public EmailTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _validator = new SendEmailCommandValidator(_mockNamesLocalizer, _mockValidationLocalizer);
    }

    [Fact]
    public void Validate_EmailIsValid_ShouldReturnSuccessResult()
    {
        // Arrange
        var email = GetValidEmailDto();

        // Act
        var result = _validator.Validate(new SendEmailCommand(email));

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_FromIsEmpty_ShouldReturnError()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["Email"]];
        var email = GetValidEmailDto();
        email.From = string.Empty;

        // Act
        var result = _validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.From)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void Validate_FromLengthIsOutOfRange_ShouldReturnError()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Email"], SendEmailCommandValidator.EmailMaxLength];
        var email = GetValidEmailDto();
        email.From = new string('e', SendEmailCommandValidator.EmailMaxLength + 2);

        // Act
        var result = _validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.From)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void Validate_EmailFormatIsInvalid_ShouldReturnError()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["EmailAddressFormat"];
        var email = GetValidEmailDto();
        email.From = "invalid////";

        // Act
        var result = _validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.From)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void Validate_TokenIsEmpty_ShouldReturnError()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["CaptchaToken"]];
        var email = GetValidEmailDto();
        email.Token = string.Empty;

        // Act
        var result = _validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.Token)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void Validate_ContentLengthIsOutOfRange_ShouldReturnError()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["LengthMustBeInRange", _mockNamesLocalizer["Content"], SendEmailCommandValidator.ContentMinLength, SendEmailCommandValidator.ContentMaxLength];
        var email = GetValidEmailDto();
        email.Content = new string('c', SendEmailCommandValidator.ContentMaxLength + 2);

        // Act
        var result = _validator.TestValidate(new SendEmailCommand(email));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.Content)
            .WithErrorMessage(expectedError);
    }

    private static EmailDTO GetValidEmailDto()
    {
        return new EmailDTO()
        {
            From = "test@gmail.com",
            Token = "token",
            Content = "Test Content",
        };
    }
}