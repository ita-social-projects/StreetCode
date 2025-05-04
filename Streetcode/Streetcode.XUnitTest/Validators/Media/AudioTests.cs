using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.MediatR.Media.Audio.Create;
using Streetcode.BLL.MediatR.Media.Audio.Update;
using Streetcode.BLL.Validators.Media.Audio;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Media;

public class AudioTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;

    public AudioTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
    }

    [Fact]
    public void ShouldReturnSuccesResult_WhenAudioIsValid()
    {
        // Arange
        var validator = new BaseAudioValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidAudioFile();

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
        var validator = new BaseAudioValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidAudioFile();
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
        var validator = new BaseAudioValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidAudioFile();
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
        var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["MimeType"]];
        var validator = new BaseAudioValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidAudioFile();
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
        var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["BaseFormat"]];
        var validator = new BaseAudioValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidAudioFile();
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
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], BaseAudioValidator.MaxTitleLength];
        var validator = new BaseAudioValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidAudioFile();
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
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["MimeType"], BaseAudioValidator.MaxMimeTypeLength];
        var validator = new BaseAudioValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidAudioFile();
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
        var expectedError = _mockValidationLocalizer["MustBeOneOf", _mockNamesLocalizer["Extension"], $"'{BaseAudioValidator.Mp3Extension}'"];
        var validator = new BaseAudioValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        var audio = GetValidAudioFile();
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
            new Mock<BaseAudioValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDTO>>()))
            .Returns(new ValidationResult());
        var createValidator = new CreateAudioValidator(baseValidator.Object);
        var createCommand = new CreateAudioCommand(GetValidAudioFile());

        // Act
        createValidator.TestValidate(createCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDTO>>()), Times.Once);
    }

    [Fact]
    public void UpdateAudioValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var baseValidator =
            new Mock<BaseAudioValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDTO>>()))
            .Returns(new ValidationResult());
        var updateValidator = new UpdateAudioValidator(baseValidator.Object);
        var updateCommand = new UpdateAudioCommand(new AudioFileBaseUpdateDTO()
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
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDTO>>()), Times.Once);
    }

    private static AudioFileBaseCreateDTO GetValidAudioFile()
    {
        return new AudioFileBaseCreateDTO()
        {
            Title = "Test Title",
            BaseFormat = "sdufhu2374jdwjfs03",
            Extension = "mp3",
            MimeType = "audio/wav",
        };
    }
}