using System.Linq.Expressions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Validators.Streetcode.ImageDetails;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using ImgDetails = Streetcode.DAL.Entities.Media.Images.ImageDetails;

namespace Streetcode.XUnitTest.Validators.Streetcode.ImageDetails;

public class ImageDetailsValidatorTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly ImageDetailsValidator _imageDetailsValidator;

    public ImageDetailsValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();

        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _imageDetailsValidator = new ImageDetailsValidator(_mockValidationLocalizer, _mockNamesLocalizer, _mockRepositoryWrapper.Object);
    }

    [Fact]
    public async Task ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        SetupRepositoryWrapper(12);
        var imageDetails = GetImageDetails(12);

        // Act
        var result = await _imageDetailsValidator.ValidateAsync(imageDetails);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldReturnError_WhenTitleLengthIsMoreThan100()
    {
        // Arrange
        SetupRepositoryWrapper(12);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["ImageTitle"], ImageDetailsValidator.TitleMaxLength];
        var imageDetails = GetImageDetails(12);
        imageDetails.Title = new string('*', ImageDetailsValidator.TitleMaxLength + 1);

        // Act
        var result = await _imageDetailsValidator.TestValidateAsync(imageDetails);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnError_WhenAltLengthIsMoreThan200()
    {
        // Arrange
        SetupRepositoryWrapper(12);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Alt"], ImageDetailsValidator.AltMaxLength];
        var imageDetails = GetImageDetails(12);
        imageDetails.Alt = new string('*', ImageDetailsValidator.AltMaxLength + 1);

        // Act
        var result = await _imageDetailsValidator.TestValidateAsync(imageDetails);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Alt)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnError_WhenExistsImageDetailsWithSameImage()
    {
        // Arrange
        SetupRepositoryWrapper(1);
        var expectedError = _mockValidationLocalizer["MustBeUnique", _mockNamesLocalizer["ImageId"]];
        var imageDetails = GetImageDetails(2);

        // Act
        var result = await _imageDetailsValidator.TestValidateAsync(imageDetails);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenImageDoesntExist()
    {
        // Arrange
        SetupRepositoryWrapper(1);
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(_mockRepositoryWrapper);
        var imageDetails = GetImageDetails(2);
        imageDetails.ImageId = 10;
        var expectedError = _mockValidationLocalizer["ImageDoesntExist", imageDetails.ImageId];

        // Act
        var result = await _imageDetailsValidator.TestValidateAsync(imageDetails);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
    }

    private void SetupRepositoryWrapper(int id)
    {
        _mockRepositoryWrapper.Setup(x => x.ImageDetailsRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ImgDetails, bool>>>(),
            It.IsAny<Func<IQueryable<ImgDetails>, IIncludableQueryable<ImgDetails, object>>>()))
            .ReturnsAsync(new ImgDetails { Id = id });

        _mockRepositoryWrapper.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<Image, bool>>>(),
            It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync(new Image { Id = id });
    }

    private static ImageDetailsDto GetImageDetails(int id)
    {
        return new ImageDetailsDto()
        {
            Id = id,
            Title = "Title",
            Alt = "1",
            ImageId = 5,
        };
    }
}