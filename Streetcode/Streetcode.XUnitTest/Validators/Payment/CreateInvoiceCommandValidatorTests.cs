using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Payment;
using Streetcode.BLL.MediatR.Payment;
using Streetcode.BLL.Validators.Payment;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Payment;

public class CreateInvoiceCommandValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly CreateInvoiceCommandValidator validator;

    public CreateInvoiceCommandValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.validator = new CreateInvoiceCommandValidator(this.mockNamesLocalizer, this.mockValidationLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenInvoiceIsValid()
    {
        // Arrange
        var command = this.GetValidCreateInvoiceCommand();

        // Act
        var result = this.validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenRedirectUrlIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["RedirectUrl"]];
        var command = this.GetValidCreateInvoiceCommand();
        command.Payment.RedirectUrl = string.Empty;

        // Act
        var result = this.validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Payment.RedirectUrl)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("ftp:://test")]
    [InlineData("https://github.com")]
    [InlineData("https://.test")]
    [InlineData("///test")]
    public void ShouldReturnError_WhenRedirectUrlIsInvalid(string invalidUrl)
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["ValidUrl", this.mockNamesLocalizer["RedirectUrl"]];
        var command = this.GetValidCreateInvoiceCommand();
        command.Payment.RedirectUrl = invalidUrl;

        // Act
        var result = this.validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Payment.RedirectUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenAmountIsLessThanZero()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["GreaterThan", this.mockNamesLocalizer["Amount"], 0];
        var command = this.GetValidCreateInvoiceCommand();
        command.Payment.Amount = -100;

        // Act
        var result = this.validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Payment.Amount)
            .WithErrorMessage(expectedError);
    }

    public CreateInvoiceCommand GetValidCreateInvoiceCommand()
    {
        return new CreateInvoiceCommand(new PaymentDto()
        {
            Amount = 100,
            RedirectUrl = "https://stage.streetcode.com.ua/support-us",
        });
    }
}