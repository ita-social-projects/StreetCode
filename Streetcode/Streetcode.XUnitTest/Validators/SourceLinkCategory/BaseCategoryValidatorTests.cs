using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Validators.SourceLinkCategory;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.SourceLinkCategory;

public class BaseCategoryValidatorTests
{
    private readonly MockFieldNamesLocalizer _mockFieldsLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

    public BaseCategoryValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockFieldsLocalizer = new MockFieldNamesLocalizer();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
    }

    [Fact]
    public async void ShouldReturnSuccessResult_WhenCategoryIsValid()
    {
        // Arrange
        var validator = new BaseCategoryValidator( _mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var validCategory = GetValidCategory();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, validCategory.ImageId);

        // Act
        var result = await validator.ValidateAsync(validCategory);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenTitleIsEmpty()
    {
        // Arrange
        var validator =
            new BaseCategoryValidator(_mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var category = GetValidCategory();
        category.Title = string.Empty;
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, category.ImageId);
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockFieldsLocalizer["Title"]];

        // Act
        var result = await validator.TestValidateAsync(category);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenTitleExceedsMaxLength()
    {
        // Arrange
        var validator =
            new BaseCategoryValidator(_mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var category = GetValidCategory();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, category.ImageId);
        category.Title = new string('a', BaseCategoryValidator.MaxCategoryLength + 1);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockFieldsLocalizer["Title"], $"{BaseCategoryValidator.MaxCategoryLength}"];

        // Act
        var result = await validator.TestValidateAsync(category);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenImageDoesntExist()
    {
        // Arrange
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(_mockRepositoryWrapper);
        var validator = new BaseCategoryValidator(_mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var category = GetValidCategory();
        category.ImageId = 10;
        var expectedError = _mockValidationLocalizer["ImageDoesntExist", category.ImageId];

        // Act
        var result = await validator.TestValidateAsync(category);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
    }

    private static SourceLinkCreateUpdateCategoryDTO GetValidCategory()
    {
        return new SourceLinkCategoryCreateDTO()
        {
            ImageId = 1,
            Title = "Test Title",
        };
    }
}