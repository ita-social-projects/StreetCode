using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Validators.Streetcode;
using Streetcode.BLL.Validators.Streetcode.Art;
using Streetcode.BLL.Validators.Streetcode.ImageDetails;
using Streetcode.BLL.Validators.Streetcode.StreetcodeArtSlide;
using Streetcode.BLL.Validators.Streetcode.TimelineItem;
using Streetcode.BLL.Validators.Streetcode.Toponyms;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode;

public class BaseStreetcodeValidatorsTests
{
    private readonly Mock<StreetcodeToponymValidator> _mockToponymValidator;
    private readonly Mock<TimelineItemValidator> _mockTimelineItemValidator;
    private readonly Mock<ImageDetailsValidator> _mockimageDetailsValidator;
    private readonly Mock<StreetcodeArtSlideValidator> _mockartSlideValidator;
    private readonly Mock<ArtCreateUpdateDTOValidator> _mockArtValidator;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly BaseStreetcodeValidator _validator;

    public BaseStreetcodeValidatorsTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockToponymValidator = new Mock<StreetcodeToponymValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        var mockContextValidator = new Mock<HistoricalContextValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        _mockTimelineItemValidator = new Mock<TimelineItemValidator>(mockContextValidator.Object, _mockValidationLocalizer, _mockNamesLocalizer);
        _mockartSlideValidator = new Mock<StreetcodeArtSlideValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockimageDetailsValidator = new Mock<ImageDetailsValidator>(_mockValidationLocalizer, _mockNamesLocalizer, mockRepositoryWrapper.Object);
        _mockArtValidator = new Mock<ArtCreateUpdateDTOValidator>(_mockValidationLocalizer, _mockNamesLocalizer);

        _validator = new BaseStreetcodeValidator(
            _mockToponymValidator.Object,
            _mockTimelineItemValidator.Object,
            _mockimageDetailsValidator.Object,
            _mockartSlideValidator.Object,
            _mockArtValidator.Object,
            _mockValidationLocalizer,
            _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var streetcode = GetValidStreetcodeDto();

        // Act
        var result = _validator.Validate(streetcode);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenFirstNameLengthIsMoreThan50()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["FirstName"], BaseStreetcodeValidator.FirstNameMaxLength];
        var streetcode = GetValidStreetcodeDto();
        streetcode.FirstName = new string('*', BaseStreetcodeValidator.FirstNameMaxLength + 1);

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLastNameLengthIsMoreThan50()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["LastName"], BaseStreetcodeValidator.LastNameMaxLength];
        var streetcode = GetValidStreetcodeDto();
        streetcode.LastName = new string('*', BaseStreetcodeValidator.LastNameMaxLength + 1);

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], BaseStreetcodeValidator.TitleMaxLength];
        var streetcode = GetValidStreetcodeDto();
        streetcode.Title = new string('*', BaseStreetcodeValidator.TitleMaxLength + 1);

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenAliasLengthIsMoreThan33()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Alias"], BaseStreetcodeValidator.AliasMaxLength];
        var streetcode = GetValidStreetcodeDto();
        streetcode.Alias = new string('*', BaseStreetcodeValidator.AliasMaxLength + 1);

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Alias)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTeaserIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["Teaser"]];
        var streetcode = GetValidStreetcodeDto();
        streetcode.Teaser = string.Empty;

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Teaser)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTeaserLengthIsMoreThan520()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Teaser"], BaseStreetcodeValidator.TeaserMaxLength];
        var streetcode = GetValidStreetcodeDto();
        streetcode.Teaser = new string('*', BaseStreetcodeValidator.TeaserMaxLength + 1);

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Teaser)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenEventStreetcodeHasNotEmpty_FirstNameAndLastName()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["EventStreetcodeCannotHasFirstName"];
        var streetcode = GetValidStreetcodeDto();
        streetcode.StreetcodeType = StreetcodeType.Event;

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTransliterationUrlIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["TransliterationUrl"]];
        var streetcode = GetValidStreetcodeDto();
        streetcode.TransliterationUrl = string.Empty;

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransliterationUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTransliterationUrlLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["TransliterationUrl"], BaseStreetcodeValidator.TransliterationUrlMaxLength];
        var streetcode = GetValidStreetcodeDto();
        streetcode.TransliterationUrl = new string('*', BaseStreetcodeValidator.TransliterationUrlMaxLength + 1);

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransliterationUrl)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("http://www.test.com")]
    [InlineData("hello.com")]
    [InlineData("тест.ком")]
    [InlineData("taras$&-shevchenko")]
    public void ShouldReturnError_WhenTransliterationUrlIsInvalid(string transliterationUrl)
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["TransliterationUrlFormat"];
        var streetcode = GetValidStreetcodeDto();
        streetcode.TransliterationUrl = transliterationUrl;

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransliterationUrl)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData(-5)]
    [InlineData(1000000)]
    public void ShouldReturnError_WhenIndexIsOutOfBounds(int index)
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MustBeBetween", _mockNamesLocalizer["Index"], BaseStreetcodeValidator.IndexMinValue, BaseStreetcodeValidator.IndexMaxValue];
        var streetcode = GetValidStreetcodeDto();
        streetcode.Index = index;

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Index)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldNotReturnError_WhenDateStringIsEmpty()
    {
        // Arrange
        var streetcode = GetValidStreetcodeDto();
        streetcode.DateString = string.Empty;

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.DateString);
    }

    [Fact]
    public void ShouldReturnError_WhenDateStringLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["DateString"], BaseStreetcodeValidator.DateStringMaxLength];
        var streetcode = GetValidStreetcodeDto();
        streetcode.DateString = new string('*', BaseStreetcodeValidator.DateStringMaxLength + 1);

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateString)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("october 2025 - december 2025")]
    [InlineData("test$^&!")]
    [InlineData("2024_2025")]
    public void ShouldReturnError_WhenDateStringIsInvalid(string invalidDateString)
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["DateStringFormat"];
        var streetcode = GetValidStreetcodeDto();
        streetcode.DateString = invalidDateString;

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateString)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenStreetcodeTypeIsInvalid()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["StreetcodeType"]];
        var streetcode = GetValidStreetcodeDto();
        streetcode.StreetcodeType = (StreetcodeType)8;

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StreetcodeType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenStatusIsInvalid()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["Status"]];
        var streetcode = GetValidStreetcodeDto();
        streetcode.Status = (StreetcodeStatus)50;

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenBlackAndWhiteImageDoesntExist()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MustContainExactlyOneAlt1", _mockNamesLocalizer["Alt"]];
        var streetcode = GetValidStreetcodeDto();
        streetcode.ImagesDetails = new List<ImageDetailsDto>();

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImagesDetails)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenThereAreTwoColoredImages()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MustContainAtMostOneAlt0", _mockNamesLocalizer["Alt"]];
        var streetcode = GetValidStreetcodeDto();
        streetcode.ImagesDetails = new List<ImageDetailsDto>()
        {
            new ImageDetailsDto()
            {
                Alt = "1",
            },
            new ImageDetailsDto()
            {
                Alt = "0",
            },
            new ImageDetailsDto()
            {
                Alt = "0",
            },
        };

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImagesDetails)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenThereAreTwoOptionalImages()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MustContainAtMostOneAlt2", _mockNamesLocalizer["Alt"]];
        var streetcode = GetValidStreetcodeDto();
        streetcode.ImagesDetails = new List<ImageDetailsDto>()
        {
            new ImageDetailsDto()
            {
                Alt = "1",
            },
            new ImageDetailsDto()
            {
                Alt = "2",
            },
            new ImageDetailsDto()
            {
                Alt = "2",
            },
        };

        // Act
        var result = _validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImagesDetails)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldCallChildValidators_WhenValidated()
    {
        // Arrange
        _mockToponymValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeToponymCreateUpdateDTO>>())).Returns(new ValidationResult());
        _mockimageDetailsValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ImageDetailsDto>>())).Returns(new ValidationResult());
        _mockTimelineItemValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<TimelineItemCreateUpdateDTO>>())).Returns(new ValidationResult());
        _mockartSlideValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeArtSlideCreateUpdateDTO>>())).Returns(new ValidationResult());
        _mockArtValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ArtCreateUpdateDTO>>())).Returns(new ValidationResult());

        var streetcode = GetValidStreetcodeDto();

        // Act
        _validator.TestValidate(streetcode);

        // Assert
        _mockToponymValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<StreetcodeToponymCreateUpdateDTO>>()), Times.AtLeast(1));
        _mockimageDetailsValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<ImageDetailsDto>>()), Times.AtLeast(1));
        _mockTimelineItemValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<TimelineItemCreateUpdateDTO>>()), Times.AtLeast(1));
        _mockartSlideValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<StreetcodeArtSlideCreateUpdateDTO>>()), Times.AtLeast(1));
        _mockArtValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<ArtCreateUpdateDTO>>()), Times.AtLeast(1));
    }

    private static StreetcodeCreateUpdateDTO GetValidStreetcodeDto()
    {
        return new StreetcodeCreateDTO()
        {
            FirstName = "Taras",
            LastName = "Shevchenko",
            Alias = "kobsar",
            Title = "Taras Shevchenko",
            Teaser = "Lorem Ipsum",
            TransliterationUrl = "taras-shevchenko",
            Index = 2,
            DateString = "25 лютого (9 березня) 1814 року – 26 лютого (10 березня) 1861 року",
            StreetcodeType = StreetcodeType.Person,
            Status = StreetcodeStatus.Published,
            EventStartOrPersonBirthDate = new DateTime(2000, 4, 2, 0, 0, 0, DateTimeKind.Local),
            Toponyms = new List<StreetcodeToponymCreateUpdateDTO>()
            {
                new (),
            },
            TimelineItems = new List<TimelineItemCreateUpdateDTO>()
            {
                new (),
            },
            ImagesDetails = new List<ImageDetailsDto>()
            {
                new ()
                {
                    Id = 1,
                    ImageId = 4,
                    Title = "Image_black&white",
                    Alt = "1",
                },
            },
            StreetcodeArtSlides = new List<StreetcodeArtSlideCreateUpdateDTO>()
            {
                new (),
            },
            Arts = new List<ArtCreateUpdateDTO>()
            {
                new (),
            },
        };
    }
}