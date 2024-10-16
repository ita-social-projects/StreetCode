using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Media.Audio.Update;
using Streetcode.BLL.MediatR.Media.Image.Create;
using Streetcode.BLL.MediatR.Media.Image.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Media.Audio;
using Streetcode.BLL.Validators.Media.Image;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Media;

public class ImageTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;

    public ImageTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
    }

    [Fact]
    public void ShouldReturnSuccesResult_WhenImageIsValid()
    {
        // Arange
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidImageFile();

        // Act
        var result = validator.Validate(audio);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleIsEmpty()
    {
        // Arange
        var expectedError = this.mockValidationLocalizer["IsRequired", this.mockNamesLocalizer["Title"]];
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidImageFile();
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
        var expectedError = this.mockValidationLocalizer["IsRequired", this.mockNamesLocalizer["Extension"]];
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var image = this.GetValidImageFile();
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
        var expectedError = this.mockValidationLocalizer["IsRequired", this.mockNamesLocalizer["MimeType"]];
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var image = this.GetValidImageFile();
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
        var expectedError = this.mockValidationLocalizer["IsRequired", this.mockNamesLocalizer["BaseFormat"]];
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var image = this.GetValidImageFile();
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
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Title"], BaseImageValidator.MaxTitleLength];
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var image = this.GetValidImageFile();
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
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Alt"], BaseImageValidator.MaxAltLength];
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var image = this.GetValidImageFile();
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
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["MimeType"], BaseImageValidator.MaxMimeTypeLength];
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var image = this.GetValidImageFile();
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
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var expectedError = this.mockValidationLocalizer["MustBeOneOf", this.mockNamesLocalizer["MimeType"], ValidationExtentions.ConcatWithComma(validator.MimeTypes)];
        var image = this.GetValidImageFile();
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
        var validator = new BaseImageValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var expectedError = this.mockValidationLocalizer["MustBeOneOf", this.mockNamesLocalizer["Extension"], ValidationExtentions.ConcatWithComma(validator.Extensions)];
        var image = this.GetValidImageFile();
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
            new Mock<BaseImageValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ImageFileBaseCreateDTO>>()))
            .Returns(new ValidationResult());
        var updateValidator = new CreateImageValidator(baseValidator.Object);
        var updateCommand = new CreateImageCommand(this.GetValidImageFile());

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
            new Mock<BaseImageValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
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

    private ImageFileBaseCreateDTO GetValidImageFile()
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