using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Media.Image.Create;
using Streetcode.BLL.MediatR.Media.Image.Update;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Media.Image;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Media;

public class ImageTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly List<string> _extensions = new () { "png", "jpeg", "jpg", "webp" };
    private readonly List<string> _mimeTypes = new () { "image/jpeg", "image/png", "image/webp" };

    public ImageTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
    }

    [Fact]
    public void ShouldReturnSuccesResult_WhenImageIsValid()
    {
        // Arange
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidImageFile();

        // Act
        var result = validator.Validate(audio);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleIsEmpty()
    {
        // Arange
        var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Title"]];
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidImageFile();
        audio.Title = string.Empty;

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenExtensionIsEmpty()
    {
        // Arange
        var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Extension"]];
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var image = GetValidImageFile();
        image.Extension = string.Empty;

        // Act
        var result = validator.TestValidate(image);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Extension)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenMimeTypeIsEmpty()
    {
        // Arange
        var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["MimeType"]];
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var image = GetValidImageFile();
        image.MimeType = string.Empty;

        // Act
        var result = validator.TestValidate(image);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MimeType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenBaseFormatIsEmpty()
    {
        // Arange
        var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["BaseFormat"]];
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var image = GetValidImageFile();
        image.BaseFormat = string.Empty;

        // Act
        var result = validator.TestValidate(image);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BaseFormat)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsOutOfRange()
    {
        // Arange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], BaseImageValidator.MaxTitleLength];
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var image = GetValidImageFile();
        image.Title = new string('a', BaseImageValidator.MaxTitleLength + 5);

        // Act
        var result = validator.TestValidate(image);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenAltLengthIsOutOfRange()
    {
        // Arange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Alt"], BaseImageValidator.MaxAltLength];
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var image = GetValidImageFile();
        image.Alt = new string('a', BaseImageValidator.MaxAltLength + 5);

        // Act
        var result = validator.TestValidate(image);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Alt)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenMimeTypeLengthIsOutOfRange()
    {
        // Arange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["MimeType"], BaseImageValidator.MaxMimeTypeLength];
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var image = GetValidImageFile();
        image.MimeType = new string('m', BaseImageValidator.MaxMimeTypeLength + 2);

        // Act
        var result = validator.TestValidate(image);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MimeType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenMimeTypeIsIncorrect()
    {
        // Arange
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var expectedError = _mockValidationLocalizer["MustBeOneOf", _mockNamesLocalizer["MimeType"], ValidationExtentions.ConcatWithComma(_mimeTypes)];
        var image = GetValidImageFile();
        image.MimeType = "video/mp4";

        // Act
        var result = validator.TestValidate(image);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MimeType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenExtensionIsIncorrect()
    {
        // Arange
        var validator = new BaseImageValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var expectedError = _mockValidationLocalizer["MustBeOneOf", _mockNamesLocalizer["Extension"], ValidationExtentions.ConcatWithComma(_extensions)];
        var image = GetValidImageFile();
        image.Extension = "mp4";

        // Act
        var result = validator.TestValidate(image);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Extension)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void CreateImageValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var baseValidator =
            new Mock<BaseImageValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ImageFileBaseCreateDTO>>()))
            .Returns(new ValidationResult());
        var updateValidator = new CreateImageValidator(baseValidator.Object);
        var updateCommand = new CreateImageCommand(GetValidImageFile());

        // Act
        updateValidator.TestValidate(updateCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<ImageFileBaseCreateDTO>>()), Times.Once);
    }

    [Fact]
    public void UpdateImageValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var baseValidator =
            new Mock<BaseImageValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ImageFileBaseCreateDTO>>()))
            .Returns(new ValidationResult());
        var updateValidator = new UpdateImageValidator(baseValidator.Object);
        var updateCommand = new UpdateImageCommand(new ImageFileBaseUpdateDTO()
        {
            Id = 1,
            Title = "Test Title",
            BaseFormat = "sdufhu2374jdwjfs03",
            Alt = "0",
            Extension = "jpeg",
            MimeType = "image/jpeg",
        });

        // Act
        updateValidator.TestValidate(updateCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<ImageFileBaseCreateDTO>>()), Times.Once);
    }

    private static ImageFileBaseCreateDTO GetValidImageFile()
    {
        return new ImageFileBaseCreateDTO()
        {
            Title = "Test Title",
            BaseFormat = "sdufhu2374jdwjfs03",
            Alt = "0",
            Extension = "jpeg",
            MimeType = "image/jpeg",
        };
    }
}