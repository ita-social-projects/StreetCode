using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update;
using Streetcode.BLL.Validators.Timeline.HistoricalContext;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Timeline;

public class UpdateHistoricalContextValidatorTests
{
    private readonly MockFieldNamesLocalizer mockFieldsLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly Mock<BaseHistoricalContextValidator> mockBaseValidator;

    public UpdateHistoricalContextValidatorTests()
    {
        this.mockFieldsLocalizer = new MockFieldNamesLocalizer();
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockBaseValidator = new Mock<BaseHistoricalContextValidator>(this.mockValidationLocalizer, this.mockFieldsLocalizer);
        this.mockBaseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<HistoricalContextDTO>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public void ShouldCallBaseValidator_WhenValidated()
    {
        // Arrange
        var updateValidator = new UpdateHistoricalContextValidator(this.mockBaseValidator.Object);
        var updateCommand = new UpdateHistoricalContextCommand(new ()
        {
            Title = "Context",
            Id = 3,
        });

        // Act
        updateValidator.TestValidate(updateCommand);

        // Assert
        this.mockBaseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<HistoricalContextDTO>>()), Times.Once);
    }
}