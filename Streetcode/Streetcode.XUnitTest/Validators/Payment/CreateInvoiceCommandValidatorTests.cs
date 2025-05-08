using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Payment;
using Streetcode.BLL.MediatR.Payment;
using Streetcode.BLL.Validators.Payment;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Payment;

public class CreateInvoiceCommandValidatorTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly CreateInvoiceCommandValidator _validator;

    public CreateInvoiceCommandValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _validator = new CreateInvoiceCommandValidator(_mockNamesLocalizer, _mockValidationLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenInvoiceIsValid()
    {
        // Arrange
        var command = GetValidCreateInvoiceCommand();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenRedirectUrlIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["RedirectUrl"]];
        var command = GetValidCreateInvoiceCommand();
        command.Payment.RedirectUrl = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Payment.RedirectUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenAmountIsLessThanZero()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["GreaterThan", _mockNamesLocalizer["Amount"], 0];
        var command = GetValidCreateInvoiceCommand();
        command.Payment.Amount = -100;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Payment.Amount)
            .WithErrorMessage(expectedError);
    }

    private static CreateInvoiceCommand GetValidCreateInvoiceCommand()
    {
        return new CreateInvoiceCommand(new PaymentDTO()
        {
            Amount = 100,
            RedirectUrl = "https://stage.streetcode.com.ua/support-us",
        });
    }
}