using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;
using Streetcode.BLL.Validators.SourceLinkCategory;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.SourceLinkCategory;

public class CreateCategoryValidatorTests
{
    private readonly MockFieldNamesLocalizer _mockFieldsLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<BaseCategoryValidator> _mockBaseValidator;

    public CreateCategoryValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockFieldsLocalizer = new MockFieldNamesLocalizer();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockBaseValidator = new Mock<BaseCategoryValidator>(_mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        _mockBaseValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<SourceLinkCreateUpdateCategoryDto>>(), default))
            .ReturnsAsync(new ValidationResult());
    }

    [Fact]
    public async Task CreateCategoryValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var createValidator = new CreateCategoryValidator(_mockBaseValidator.Object, _mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var createCommand = GetValidUpdateCategoryCommand();
        MockHelpers.SetupMockSourceCategoryRepositoryWithExistingCategory(_mockRepositoryWrapper, createCommand.Category.Title);

        // Act
        await createValidator.TestValidateAsync(createCommand);

        // Assert
        _mockBaseValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<SourceLinkCreateUpdateCategoryDto>>(), default), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnSuccessResult_WhenUpdateCategoryCommandIsValid()
    {
        // Arrange
        var updateValidator = new CreateCategoryValidator(_mockBaseValidator.Object, _mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var createCommand = GetValidUpdateCategoryCommand();
        MockHelpers.SetupMockSourceCategoryRepositoryWithoutExistingCategory(_mockRepositoryWrapper);

        // Act
        var result = await updateValidator.TestValidateAsync(createCommand);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenCategoryWithSameTitleExists()
    {
        // Arrange
        var updateValidator = new CreateCategoryValidator(_mockBaseValidator.Object, _mockRepositoryWrapper.Object, _mockValidationLocalizer, _mockFieldsLocalizer);
        var createCommand = GetValidUpdateCategoryCommand();
        MockHelpers.SetupMockSourceCategoryRepositoryWithExistingCategory(_mockRepositoryWrapper, createCommand.Category.Title);
        var expectedError = _mockValidationLocalizer["MustBeUnique", _mockFieldsLocalizer["Title"]];

        // Act
        var result = await updateValidator.TestValidateAsync(createCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Category)
            .WithErrorMessage(expectedError);
    }

    private static CreateCategoryCommand GetValidUpdateCategoryCommand()
    {
        return new CreateCategoryCommand(new SourceLinkCategoryCreateDto()
        {
            Title = "Test Title",
            ImageId = 1,
        });
    }
}