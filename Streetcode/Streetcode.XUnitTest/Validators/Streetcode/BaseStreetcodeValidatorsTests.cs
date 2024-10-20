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
    private readonly Mock<StreetcodeToponymValidator> mockToponymValidator;
    private readonly Mock<TimelineItemValidator> mockTimelineItemValidator;
    private readonly Mock<ImageDetailsValidator> mockimageDetailsValidator;
    private readonly Mock<StreetcodeArtSlideValidator> mockartSlideValidator;
    private readonly Mock<ArtCreateUpdateDTOValidator> mockArtValidator;
    private MockFailedToValidateLocalizer mockValidationLocalizer;
    private MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly BaseStreetcodeValidator validator;

    public BaseStreetcodeValidatorsTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.mockToponymValidator = new Mock<StreetcodeToponymValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var mockContextValidator = new Mock<HistoricalContextValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        this.mockTimelineItemValidator = new Mock<TimelineItemValidator>(mockContextValidator.Object, this.mockValidationLocalizer, this.mockNamesLocalizer);
        this.mockartSlideValidator = new Mock<StreetcodeArtSlideValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        this.mockimageDetailsValidator = new Mock<ImageDetailsValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer, mockRepositoryWrapper.Object);
        this.mockArtValidator = new Mock<ArtCreateUpdateDTOValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);

        this.validator = new BaseStreetcodeValidator(
            this.mockToponymValidator.Object,
            this.mockTimelineItemValidator.Object,
            this.mockimageDetailsValidator.Object,
            this.mockartSlideValidator.Object,
            this.mockArtValidator.Object,
            this.mockValidationLocalizer,
            this.mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var streetcode = this.GetValidStreetcodeDto();

        // Act
        var result = this.validator.Validate(streetcode);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenFirstNameLengthIsMoreThan50()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["FirstName"], BaseStreetcodeValidator.FirstNameMaxLength];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.FirstName = new string('*', BaseStreetcodeValidator.FirstNameMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLastNameLengthIsMoreThan50()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["LastName"], BaseStreetcodeValidator.LastNameMaxLength];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.LastName = new string('*', BaseStreetcodeValidator.LastNameMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Title"], BaseStreetcodeValidator.TitleMaxLength];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.Title = new string('*', BaseStreetcodeValidator.TitleMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenAliasLengthIsMoreThan33()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Alias"], BaseStreetcodeValidator.AliasMaxLength];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.Alias = new string('*', BaseStreetcodeValidator.AliasMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Alias)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTeaserIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["Teaser"]];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.Teaser = string.Empty;

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Teaser)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTeaserLengthIsMoreThan520()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Teaser"], BaseStreetcodeValidator.TeaserMaxLength];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.Teaser = new string('*', BaseStreetcodeValidator.TeaserMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Teaser)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenEventStreetcodeHasNotEmpty_FirstNameAndLastName()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["EventStreetcodeCannotHasFirstName"];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.StreetcodeType = StreetcodeType.Event;

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTransliterationUrlIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["TransliterationUrl"]];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.TransliterationUrl = string.Empty;

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransliterationUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTransliterationUrlLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["TransliterationUrl"], BaseStreetcodeValidator.TransliterationUrlMaxLength];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.TransliterationUrl = new string('*', BaseStreetcodeValidator.TransliterationUrlMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(streetcode);

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
        var expectedError = this.mockValidationLocalizer["TransliterationUrlFormat"];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.TransliterationUrl = transliterationUrl;

        // Act
        var result = this.validator.TestValidate(streetcode);

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
        var expectedError = this.mockValidationLocalizer["MustBeBetween", this.mockNamesLocalizer["Index"], BaseStreetcodeValidator.IndexMinValue, BaseStreetcodeValidator.IndexMaxValue];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.Index = index;

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Index)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenDateStringIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["DateString"]];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.DateString = string.Empty;

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateString)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenDateStringLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["DateString"], BaseStreetcodeValidator.DateStringMaxLength];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.DateString = new string('*', BaseStreetcodeValidator.DateStringMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(streetcode);

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
        var expectedError = this.mockValidationLocalizer["DateStringFormat"];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.DateString = invalidDateString;

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateString)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenStreetcodeTypeIsInvalid()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["Invalid", this.mockNamesLocalizer["StreetcodeType"]];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.StreetcodeType = (StreetcodeType)8;

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StreetcodeType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenStatusIsInvalid()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["Invalid", this.mockNamesLocalizer["Status"]];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.Status = (StreetcodeStatus)50;

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenBlackAndWhiteImageDoesntExist()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MustContainExactlyOneAlt1", this.mockNamesLocalizer["Alt"]];
        var streetcode = this.GetValidStreetcodeDto();
        streetcode.ImagesDetails = new List<ImageDetailsDto>();

        // Act
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImagesDetails)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenThereAreTwoColoredImages()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MustContainAtMostOneAlt0", this.mockNamesLocalizer["Alt"]];
        var streetcode = this.GetValidStreetcodeDto();
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
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImagesDetails)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenThereAreTwoOptionalImages()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MustContainAtMostOneAlt2", this.mockNamesLocalizer["Alt"]];
        var streetcode = this.GetValidStreetcodeDto();
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
        var result = this.validator.TestValidate(streetcode);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImagesDetails)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldCallChildValidators_WhenValidated()
    {
        // Arrange
        this.mockToponymValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeToponymCreateUpdateDTO>>())).Returns(new ValidationResult());
        this.mockimageDetailsValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ImageDetailsDto>>())).Returns(new ValidationResult());
        this.mockTimelineItemValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<TimelineItemCreateUpdateDTO>>())).Returns(new ValidationResult());
        this.mockartSlideValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeArtSlideCreateUpdateDTO>>())).Returns(new ValidationResult());
        this.mockArtValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ArtCreateUpdateDTO>>())).Returns(new ValidationResult());

        var streetcode = this.GetValidStreetcodeDto();

        // Act
        this.validator.TestValidate(streetcode);

        // Assert
        this.mockToponymValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<StreetcodeToponymCreateUpdateDTO>>()), Times.AtLeast(1));
        this.mockimageDetailsValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<ImageDetailsDto>>()), Times.AtLeast(1));
        this.mockTimelineItemValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<TimelineItemCreateUpdateDTO>>()), Times.AtLeast(1));
        this.mockartSlideValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<StreetcodeArtSlideCreateUpdateDTO>>()), Times.AtLeast(1));
        this.mockArtValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<ArtCreateUpdateDTO>>()), Times.AtLeast(1));
    }

    private StreetcodeCreateUpdateDTO GetValidStreetcodeDto()
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