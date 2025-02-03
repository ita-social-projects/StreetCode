using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.MediatR.Media.Audio.Create;
using Streetcode.BLL.MediatR.Media.Audio.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Media.Audio;
using Streetcode.BLL.Validators.Streetcode.Subtitles;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Media;

public class AudioTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;

    public AudioTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
    }

    [Fact]
    public void ShouldReturnSuccesResult_WhenAudioIsValid()
    {
        // Arange
        var validator = new BaseAudioValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidAudioFile();

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
        var validator = new BaseAudioValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidAudioFile();
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
        var validator = new BaseAudioValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidAudioFile();
        audio.Extension = string.Empty;

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Extension)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenMimeTypeIsEmpty()
    {
        // Arange
        var expectedError = this.mockValidationLocalizer["IsRequired", this.mockNamesLocalizer["MimeType"]];
        var validator = new BaseAudioValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidAudioFile();
        audio.MimeType = string.Empty;

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MimeType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenBaseFormatIsEmpty()
    {
        // Arange
        var expectedError = this.mockValidationLocalizer["IsRequired", this.mockNamesLocalizer["BaseFormat"]];
        var validator = new BaseAudioValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidAudioFile();
        audio.BaseFormat = string.Empty;

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BaseFormat)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsOutOfRange()
    {
        // Arange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Title"], BaseAudioValidator.MaxTitleLength];
        var validator = new BaseAudioValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidAudioFile();
        audio.Title = new string('a', BaseAudioValidator.MaxTitleLength + 1);

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenMimeTypeLengthIsOutOfRange()
    {
        // Arange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["MimeType"], BaseAudioValidator.MaxMimeTypeLength];
        var validator = new BaseAudioValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidAudioFile();
        audio.MimeType = new string('a', BaseAudioValidator.MaxMimeTypeLength + 1);

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MimeType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenExtensionIsNotMp3()
    {
        // Arange
        var expectedError = this.mockValidationLocalizer["MustBeOneOf", this.mockNamesLocalizer["Extension"], $"'{BaseAudioValidator.Mp3Extension}'"];
        var validator = new BaseAudioValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var audio = this.GetValidAudioFile();
        audio.Extension = "jpeg";

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Extension)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void CreateAudioValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var baseValidator =
            new Mock<BaseAudioValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDto>>()))
            .Returns(new ValidationResult());
        var createValidator = new CreateAudioValidator(baseValidator.Object);
        var createCommand = new CreateAudioCommand(this.GetValidAudioFile());

        // Act
        createValidator.TestValidate(createCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDto>>()), Times.Once);
    }

    [Fact]
    public void UpdateAudioValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var baseValidator =
            new Mock<BaseAudioValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDto>>()))
            .Returns(new ValidationResult());
        var updateValidator = new UpdateAudioValidator(baseValidator.Object);
        var updateCommand = new UpdateAudioCommand(new AudioFileBaseUpdateDto()
        {
            Id = 1,
            Title = "New Title",
            BaseFormat = "dkdpkdmy2734hdnf",
            MimeType = "audio/mpeg",
            Extension = "mp3",
        });

        // Act
        updateValidator.TestValidate(updateCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDto>>()), Times.Once);
    }

    private AudioFileBaseCreateDto GetValidAudioFile()
    {
        return new AudioFileBaseCreateDto()
        {
            Title = "Test Title",
            BaseFormat = "sdufhu2374jdwjfs03",
            Extension = "mp3",
            MimeType = "audio/wav",
        };
    }
}