using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Create;
using Streetcode.BLL.Validators.Timeline.HistoricalContext;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Timeline;

public class CreateHistoricalContextValidatorTests
{
    private readonly MockFieldNamesLocalizer mockFieldsLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly Mock<BaseHistoricalContextValidator> mockBaseValidator;

    public CreateHistoricalContextValidatorTests()
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
        var createValidator = new CreateHistoricalContextValidator(this.mockBaseValidator.Object);
        var createCommand = new CreateHistoricalContextCommand(new ()
        {
            Title = "Context",
            Id = 3,
        });

        // Act
        createValidator.TestValidate(createCommand);

        // Assert
        this.mockBaseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<HistoricalContextDTO>>()), Times.Once);
    }
}