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
    private readonly MockFieldNamesLocalizer _mockFieldsLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly Mock<BaseHistoricalContextValidator> _mockBaseValidator;

    public CreateHistoricalContextValidatorTests()
    {
        _mockFieldsLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockBaseValidator = new Mock<BaseHistoricalContextValidator>(_mockValidationLocalizer, _mockFieldsLocalizer);
        _mockBaseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<HistoricalContextDTO>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public void ShouldCallBaseValidator_WhenValidated()
    {
        // Arrange
        var createValidator = new CreateHistoricalContextValidator(_mockBaseValidator.Object);
        var createCommand = new CreateHistoricalContextCommand(new ()
        {
            Title = "Context",
            Id = 3,
        });

        // Act
        createValidator.TestValidate(createCommand);

        // Assert
        _mockBaseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<HistoricalContextDTO>>()), Times.Once);
    }
}