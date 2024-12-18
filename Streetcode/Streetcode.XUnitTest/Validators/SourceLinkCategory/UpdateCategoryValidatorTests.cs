using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLink.Update;
using Streetcode.BLL.Validators.SourceLinkCategory;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.SourceLinkCategory;

public class UpdateCategoryValidatorTests
{
    private readonly MockFieldNamesLocalizer _mockFieldsLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<BaseCategoryValidator> _mockBaseValidator;

    public UpdateCategoryValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockFieldsLocalizer = new MockFieldNamesLocalizer();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockBaseValidator = new Mock<BaseCategoryValidator>(_mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        _mockBaseValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<SourceLinkCreateUpdateCategoryDTO>>(), default))
            .ReturnsAsync(new ValidationResult());
    }

    [Fact]
    public async Task UpdateCategoryValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var updateValidator = new UpdateCategoryValidator(_mockBaseValidator.Object, _mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var updateCommand = GetValidUpdateCategoryCommand();
        MockHelpers.SetupMockSourceCategoryRepositoryWithExistingCategory(_mockRepositoryWrapper, updateCommand.Category.Title);

        // Act
        await updateValidator.TestValidateAsync(updateCommand);

        // Assert
        _mockBaseValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<SourceLinkCreateUpdateCategoryDTO>>(), default), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnSuccessResult_WhenUpdateCategoryCommandIsValid()
    {
        // Arrange
        var updateValidator = new UpdateCategoryValidator(_mockBaseValidator.Object, _mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var updateCommand = GetValidUpdateCategoryCommand();
        MockHelpers.SetupMockSourceCategoryRepositoryWithoutExistingCategory(_mockRepositoryWrapper);

        // Act
        var result = await updateValidator.TestValidateAsync(updateCommand);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenCategoryWithSameTitleExists()
    {
        // Arrange
        var updateValidator = new UpdateCategoryValidator(_mockBaseValidator.Object, _mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var updateCommand = GetValidUpdateCategoryCommand();
        MockHelpers.SetupMockSourceCategoryRepositoryWithExistingCategory(_mockRepositoryWrapper, updateCommand.Category.Title);
        var expectedError = _mockValidationLocalizer["MustBeUnique", _mockFieldsLocalizer["Title"]];

        // Act
        var result = await updateValidator.TestValidateAsync(updateCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Category)
            .WithErrorMessage(expectedError);
    }

    private static UpdateCategoryCommand GetValidUpdateCategoryCommand()
    {
        return new UpdateCategoryCommand(new UpdateSourceLinkCategoryDTO()
        {
            Id = 1,
            Title = "Test Title",
            ImageId = 1,
        });
    }
}