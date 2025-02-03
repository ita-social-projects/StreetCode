using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Create;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Update;
using Streetcode.BLL.Validators.AdditionalContent.Tag;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Streetcode.XUnitTest.Validators.AdditionalContent.Tags;

public class TagsTests
{
    private readonly MockFieldNamesLocalizer _mockFieldNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;

    public TagsTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockFieldNamesLocalizer = new MockFieldNamesLocalizer();
    }

    [Fact]
    public async void ShouldReturnSuccessResult_WhenNewsIsValid()
    {
        // Arrange
        var validator = new BaseTagValidator(_mockValidationLocalizer, _mockFieldNamesLocalizer);
        var validNews = GetValidTag();

        // Act
        var result = await validator.ValidateAsync(validNews);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    public void ShouldReturnValidationError_WhenTitleIsEmpty(string invalidTitle)
    {
        // Arrange
        var validator = new BaseTagValidator(_mockValidationLocalizer, _mockFieldNamesLocalizer);
        var tag = GetValidTag();
        tag.Title = invalidTitle;
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockFieldNamesLocalizer["Title"]];

        // Act
        var result = validator.TestValidate(tag);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenTitleExceedsMaxLength()
    {
        // Arrange
        var validator = new BaseTagValidator(_mockValidationLocalizer, _mockFieldNamesLocalizer);
        var tag = GetValidTag();
        tag.Title = new string('a', BaseTagValidator.TitleMaxLength + 1);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockFieldNamesLocalizer["Title"], BaseTagValidator.TitleMaxLength];

        // Act
        var result = validator.TestValidate(tag);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void UpdateTagValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var baseValidator = new Mock<BaseTagValidator>(_mockValidationLocalizer, _mockFieldNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreateUpdateTagDto>>()))
            .Returns(new ValidationResult());

        var updateValidator = new UpdateTagValidator(baseValidator.Object);
        var updateCommand = new UpdateTagCommand(new UpdateTagDto()
        {
            Id = 1,
            Title = "Test Title",
        });

        // Act
        var result = updateValidator.TestValidate(updateCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<CreateUpdateTagDto>>()), Times.Once);
    }

    [Fact]
    public void CreateTagValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var baseValidator = new Mock<BaseTagValidator>(_mockValidationLocalizer, _mockFieldNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreateUpdateTagDto>>()))
            .Returns(new ValidationResult());

        var createValidator = new CreateTagValidator(baseValidator.Object);
        var createCommand = new CreateTagQuery(new CreateTagDto()
        {
            Title = "Test Title",
        });

        // Act
        var result = createValidator.TestValidate(createCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<CreateUpdateTagDto>>()), Times.Once);
    }

    private static CreateUpdateTagDto GetValidTag()
    {
        return new CreateTagDto()
        {
            Title = "Test Title",
        };
    }
}
