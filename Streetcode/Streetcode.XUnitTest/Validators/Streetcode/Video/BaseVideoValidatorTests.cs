using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Validators.Streetcode.Video;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Video;

public class BaseVideoValidatorTests
{
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly BaseVideoValidator _validator;

    public BaseVideoValidatorTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _validator = new BaseVideoValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenTextCreateDTOIsValid()
    {
        // Arrange
        var validVideo = GetValidVideo();

        // Act
        var result = _validator.TestValidate(validVideo);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenUrlIsEmpty()
    {
        // Arrange
        var video = GetValidVideo();
        video.Url = string.Empty;
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["Video"]];

        // Act
        var result = _validator.TestValidate(video);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Url)
            .WithErrorMessage(expectedError);
    }

    [InlineData("https://streetcode.com.ua/")]
    [InlineData("https://stage.streetcode.com.ua/")]
    [Theory]
    public void ShouldReturnValidationError_WhenUrlIsInvalid(string invalidUrl)
    {
        // Arrange
        var video = GetValidVideo();
        video.Url = string.Empty;
        var expectedError = _mockValidationLocalizer["ValidUrl", _mockNamesLocalizer["Video"]];

        // Act
        var result = _validator.TestValidate(video);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Url)
            .WithErrorMessage(expectedError);
    }

    [InlineData("https://www.youtube.com/")]
    [InlineData("https://youtu.be/LDwDUIjb93Q?si=SgdPRgaEoHDp0BDn")]
    [Theory]
    public void ShouldntReturnValidationError_WhenUrlIsValid(string invalidUrl)
    {
        // Arrange
        var video = GetValidVideo();
        video.Url = invalidUrl;

        // Act
        var result = _validator.TestValidate(video);

        // Assert
        result.ShouldNotHaveValidationErrorFor(dto => dto.Url);
    }

    public static VideoCreateUpdateDTO GetValidVideo()
    {
        return new VideoCreateDTO()
        {
            Description = "Description Test",
            Url = "https://www.youtube.com/",
        };
    }
}