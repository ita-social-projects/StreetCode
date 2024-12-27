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
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly Mock<IRepositoryWrapper> mockRepositoryWrapper;
    private readonly ImageDetailsValidator imageDetailsValidator;

    public ImageDetailsValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();

        this.mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        this.imageDetailsValidator = new ImageDetailsValidator(this.mockValidationLocalizer, this.mockNamesLocalizer, this.mockRepositoryWrapper.Object);
    }

    [Fact]
    public async Task ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        this.SetupRepositoryWrapper(12);
        var imageDetails = this.GetImageDetails(12);

        // Act
        var result = await this.imageDetailsValidator.ValidateAsync(imageDetails);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldReturnError_WhenTitleLengthIsMoreThan100()
    {
        // Arrange
        this.SetupRepositoryWrapper(12);
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["ImageTitle"], ImageDetailsValidator.TitleMaxLength];
        var imageDetails = this.GetImageDetails(12);
        imageDetails.Title = new string('*', ImageDetailsValidator.TitleMaxLength + 1);

        // Act
        var result = await this.imageDetailsValidator.TestValidateAsync(imageDetails);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnError_WhenAltLengthIsMoreThan200()
    {
        // Arrange
        this.SetupRepositoryWrapper(12);
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Alt"], ImageDetailsValidator.AltMaxLength];
        var imageDetails = this.GetImageDetails(12);
        imageDetails.Alt = new string('*', ImageDetailsValidator.AltMaxLength + 1);

        // Act
        var result = await this.imageDetailsValidator.TestValidateAsync(imageDetails);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Alt)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnError_WhenExistsImageDetailsWithSameImage()
    {
        // Arrange
        this.SetupRepositoryWrapper(1);
        var expectedError = this.mockValidationLocalizer["MustBeUnique", this.mockNamesLocalizer["ImageId"]];
        var imageDetails = this.GetImageDetails(2);

        // Act
        var result = await this.imageDetailsValidator.TestValidateAsync(imageDetails);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenImageDoesntExist()
    {
        // Arrange
        this.SetupRepositoryWrapper(1);
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(mockRepositoryWrapper);
        var imageDetails = GetImageDetails(2);
        imageDetails.ImageId = 10;
        var expectedError = mockValidationLocalizer["ImageDoesntExist", imageDetails.ImageId];

        // Act
        var result = await imageDetailsValidator.TestValidateAsync(imageDetails);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
    }

    private void SetupRepositoryWrapper(int id)
    {
        this.mockRepositoryWrapper.Setup(x => x.ImageDetailsRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ImgDetails, bool>>>(),
            It.IsAny<Func<IQueryable<ImgDetails>, IIncludableQueryable<ImgDetails, object>>>()))
            .ReturnsAsync(new ImgDetails { Id = id });
        
        this.mockRepositoryWrapper.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<Image, bool>>>(),
            It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync(new Image { Id = id });
    }

    private ImageDetailsDto GetImageDetails(int id)
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