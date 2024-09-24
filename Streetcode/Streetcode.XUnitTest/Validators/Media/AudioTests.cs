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
using Xunit;

namespace Streetcode.XUnitTest.Validators.Media;

public class AudioTests
{
    private readonly Mock<IStringLocalizer<FailedToValidateSharedResource>> mockValidationLocalizer;
    private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> mockNamesLocalizer;

    private const string testRequiredError = "Field is required";
    private const string testMaxLengthError = "Value must be less than {0} characters long";
    private const string testMustBeOneOfError = "Value must be one of: {0}";

    public AudioTests()
    {
        this.mockValidationLocalizer = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
        this.mockNamesLocalizer = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
    }

    [Fact]
    public void ShouldReturnSuccesResult_WhenAudioIsValid()
    {
        // Arange
        this.SetupLocalizers();
        var validator = new BaseAudioValidator(mockValidationLocalizer.Object, mockNamesLocalizer.Object);
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
        this.SetupLocalizers();
        var validator = new BaseAudioValidator(mockValidationLocalizer.Object, mockNamesLocalizer.Object);
        var audio = this.GetValidAudioFile();
        audio.Title = string.Empty;

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(audio => audio.Title)
            .WithErrorMessage(testRequiredError);
    }

    [Fact]
    public void ShouldReturnError_WhenExtensionIsEmpty()
    {
        // Arange
        this.SetupLocalizers();
        var validator = new BaseAudioValidator(mockValidationLocalizer.Object, mockNamesLocalizer.Object);
        var audio = this.GetValidAudioFile();
        audio.Extension = string.Empty;

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(audio => audio.Extension)
            .WithErrorMessage(testRequiredError);
    }

    [Fact]
    public void ShouldReturnError_WhenMimeTypeIsEmpty()
    {
        // Arange
        this.SetupLocalizers();
        var validator = new BaseAudioValidator(mockValidationLocalizer.Object, mockNamesLocalizer.Object);
        var audio = this.GetValidAudioFile();
        audio.MimeType = string.Empty;

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(audio => audio.MimeType)
            .WithErrorMessage(testRequiredError);
    }

    [Fact]
    public void ShouldReturnError_WhenBaseFormatIsEmpty()
    {
        // Arange
        this.SetupLocalizers();
        var validator = new BaseAudioValidator(mockValidationLocalizer.Object, mockNamesLocalizer.Object);
        var audio = this.GetValidAudioFile();
        audio.BaseFormat = string.Empty;

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(audio => audio.BaseFormat)
            .WithErrorMessage(testRequiredError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsOutOfRange()
    {
        // Arange
        this.SetupLocalizers(BaseAudioValidator.MaxTitleLength);
        var validator = new BaseAudioValidator(mockValidationLocalizer.Object, mockNamesLocalizer.Object);
        var audio = this.GetValidAudioFile();
        audio.Title = new string('a', BaseAudioValidator.MaxTitleLength + 1);

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(audio => audio.Title)
            .WithErrorMessage(string.Format(testMaxLengthError, BaseAudioValidator.MaxTitleLength));
    }

    [Fact]
    public void ShouldReturnError_WhenMimeTypeLengthIsOutOfRange()
    {
        // Arange
        this.SetupLocalizers(BaseAudioValidator.MaxMimeTypeLength);
        var validator = new BaseAudioValidator(mockValidationLocalizer.Object, mockNamesLocalizer.Object);
        var audio = this.GetValidAudioFile();
        audio.MimeType = new string('a', BaseAudioValidator.MaxMimeTypeLength + 1);

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(audio => audio.MimeType)
            .WithErrorMessage(string.Format(testMaxLengthError, BaseAudioValidator.MaxMimeTypeLength));
    }

    [Fact]
    public void ShouldReturnError_WhenExtensionIsNotMp3()
    {
        // Arange
        this.SetupLocalizers(0, BaseAudioValidator.Mp3Extension);
        var validator = new BaseAudioValidator(mockValidationLocalizer.Object, mockNamesLocalizer.Object);
        var audio = this.GetValidAudioFile();
        audio.Extension = "jpeg";

        // Act
        var result = validator.TestValidate(audio);

        // Assert
        result.ShouldHaveValidationErrorFor(audio => audio.Extension)
            .WithErrorMessage(string.Format(testMustBeOneOfError, BaseAudioValidator.Mp3Extension));
    }

    [Fact]
    public void CreateAudioValidator_ShouldCallBaseValidator()
    {
        // Arrange
        this.SetupLocalizers();
        var baseValidator = new Mock<BaseAudioValidator>(this.mockValidationLocalizer.Object, this.mockNamesLocalizer.Object);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDTO>>()))
            .Returns(new ValidationResult());
        var createValidator = new CreateAudioValidator(baseValidator.Object);
        var createCommand = new CreateAudioCommand(this.GetValidAudioFile());

        // Act
        var result = createValidator.TestValidate(createCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDTO>>()), Times.Once);
    }

    [Fact]
    public void UpdateAudioValidator_ShouldCallBaseValidator()
    {
        // Arrange
        this.SetupLocalizers();
        var baseValidator = new Mock<BaseAudioValidator>(this.mockValidationLocalizer.Object, this.mockNamesLocalizer.Object);
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
        var result = updateValidator.TestValidate(updateCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<AudioFileBaseCreateDTO>>()), Times.Once);
    }

    private AudioFileBaseCreateDTO GetValidAudioFile()
    {
        return new AudioFileBaseCreateDTO()
        {
            Title = "Test Title",
            BaseFormat = "sdufhu2374jdwjfs03",
            Extension = "mp3",
            MimeType = "audio/wav",
        };
    }

    private void SetupLocalizers(int maxLength = 0, string oneOfArgument = "")
    {
        this.mockValidationLocalizer.Setup(x => x["IsRequired", It.IsAny<object[]>()])
            .Returns(new LocalizedString("IsRequired", testRequiredError));
        this.mockValidationLocalizer.Setup(x => x["MaxLength", It.IsAny<object[]>()])
            .Returns(new LocalizedString("MaxLength", string.Format(testMaxLengthError, maxLength)));
        this.mockValidationLocalizer.Setup(x => x["MustBeOneOf", It.IsAny<object[]>()])
            .Returns(new LocalizedString("MustBeOneOf", string.Format(testMustBeOneOfError, oneOfArgument)));
    }
}