using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Sources.Update;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Enums;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
using Streetcode.BLL.Validators.AdditionalContent.Tag;
using Streetcode.BLL.Validators.Streetcode;
using Streetcode.BLL.Validators.Streetcode.Art;
using Streetcode.BLL.Validators.Streetcode.CategoryContent;
using Streetcode.BLL.Validators.Streetcode.Facts;
using Streetcode.BLL.Validators.Streetcode.ImageDetails;
using Streetcode.BLL.Validators.Streetcode.StreetcodeArtSlide;
using Streetcode.BLL.Validators.Streetcode.Subtitles;
using Streetcode.BLL.Validators.Streetcode.Text;
using Streetcode.BLL.Validators.Streetcode.TimelineItem;
using Streetcode.BLL.Validators.Streetcode.Toponyms;
using Streetcode.BLL.Validators.Streetcode.Video;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode;

public class UpdateStreetcodeValidatorTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly Mock<IRepositoryWrapper> _repositoryWrapper;
    private readonly Mock<BaseStreetcodeValidator> _baseStreetcodeValidator;
    private readonly Mock<BaseSubtitleValidator> _baseSubtitleValidator;
    private readonly Mock<BaseTextValidator> _baseTextValidator;
    private readonly Mock<BaseTagValidator> _tagValidator;
    private readonly Mock<BaseFactValidator> _baseFactValidator;
    private readonly Mock<BaseVideoValidator> _videoValidator;
    private readonly Mock<BaseCategoryContentValidator> _categoryContentValidator;
    private readonly UpdateStreetcodeValidator _validator;

    public UpdateStreetcodeValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _categoryContentValidator = new Mock<BaseCategoryContentValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        _videoValidator = new Mock<BaseVideoValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        _baseTextValidator = new Mock<BaseTextValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        _baseFactValidator = new Mock<BaseFactValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        _tagValidator = new Mock<BaseTagValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        _baseSubtitleValidator = new Mock<BaseSubtitleValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        _repositoryWrapper = new Mock<IRepositoryWrapper>();
        var mockToponymValidator = new Mock<StreetcodeToponymValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        var mockContextValidator = new Mock<HistoricalContextValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        var mockTimelineItemValidator = new Mock<TimelineItemValidator>(mockContextValidator.Object, _mockValidationLocalizer, _mockNamesLocalizer);
        var mockartSlideValidator = new Mock<StreetcodeArtSlideValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        var mockimageDetailsValidator = new Mock<ImageDetailsValidator>(_mockValidationLocalizer, _mockNamesLocalizer, _repositoryWrapper.Object);
        var mockArtValidator = new Mock<ArtCreateUpdateDTOValidator>(_mockValidationLocalizer, _mockNamesLocalizer);

        _baseStreetcodeValidator = new Mock<BaseStreetcodeValidator>(
            mockToponymValidator.Object,
            mockTimelineItemValidator.Object,
            mockimageDetailsValidator.Object,
            mockartSlideValidator.Object,
            mockArtValidator.Object,
            _mockValidationLocalizer,
            _mockNamesLocalizer);

        _validator = new UpdateStreetcodeValidator(
            _repositoryWrapper.Object,
            _baseStreetcodeValidator.Object,
            _baseTextValidator.Object,
            _baseSubtitleValidator.Object,
            _tagValidator.Object,
            _baseFactValidator.Object,
            _videoValidator.Object,
            _categoryContentValidator.Object,
            _mockValidationLocalizer,
            _mockNamesLocalizer);
    }

    [Fact]
    public async Task ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        SetupRepositoryWrapperReturnsNull();
        var command = GetValidCreateStreetcodeCommand();

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldReturnError_WhenIndexIsNotUnique()
    {
        // Arrange
        SetupRepositoryWrapper(1);
        var expectedError = _mockValidationLocalizer["MustBeUnique", _mockNamesLocalizer["Index"]];
        var command = GetValidCreateStreetcodeCommand();

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcode)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("testcom")]
    [InlineData("////test/com")]
    [InlineData("ftp://test.com")]
    public async Task ShouldReturnError_WhenArUrlIsInvalid(string invalidUrl)
    {
        // Arrange
        SetupRepositoryWrapperReturnsNull();
        var expectedError = _mockValidationLocalizer["ValidUrl", _mockNamesLocalizer["ARBlockURL"]];
        var command = GetValidCreateStreetcodeCommand();
        command.Streetcode.ArBlockUrl = invalidUrl;

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcode.ArBlockUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnError_WhenExistsVideosWithoutTitle()
    {
        // Arrange
        SetupRepositoryWrapperReturnsNull();
        var expectedError = _mockValidationLocalizer["CannotBeEmptyWithCondition", _mockNamesLocalizer["Title"], _mockNamesLocalizer["Video"]];
        var command = GetValidCreateStreetcodeCommand();
        command.Streetcode.Text = new TextUpdateDTO()
        {
            Title = string.Empty,
        };
        command.Streetcode.Videos = new List<VideoUpdateDTO>()
        {
            new VideoUpdateDTO()
            {
                Url = "uuuurl",
            },
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcode.Text!.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldCallBaseValidator_WhenValidated()
    {
        // Arrange
        SetupRepositoryWrapperReturnsNull();
        SetupValidatorMocks();
        var command = GetValidCreateStreetcodeCommand();

        // Act
        await _validator.TestValidateAsync(command);

        // Assert
        _baseStreetcodeValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<StreetcodeCreateUpdateDTO>>(), default), Times.AtLeast(1));
    }

    [Fact]
    public async Task ShouldCallChildValidators_WhenValidated()
    {
        // Arrange
        SetupRepositoryWrapperReturnsNull();
        SetupValidatorMocks();
        var command = GetValidCreateStreetcodeCommand();

        // Act
        await _validator.TestValidateAsync(command);

        // Assert
        _categoryContentValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<StreetcodeCategoryContentDTO>>(), default), Times.AtLeast(1));
        _videoValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<VideoCreateUpdateDTO>>(), default), Times.AtLeast(1));
        _baseTextValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<BaseTextDTO>>(), default), Times.AtLeast(1));
        _baseFactValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<FactUpdateCreateDto>>(), default), Times.AtLeast(1));
        _tagValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateUpdateTagDTO>>(), default), Times.AtLeast(1));
        _baseSubtitleValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<SubtitleCreateUpdateDTO>>(), default), Times.AtLeast(1));
    }

    [Fact]
    public async Task ShouldReturnError_WhenTryToUpdateTag_WithId_0()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["Id"]];
        SetupRepositoryWrapperReturnsNull();
        SetupValidatorMocks();
        var command = GetValidCreateStreetcodeCommand();
        command.Streetcode.Tags = new List<StreetcodeTagUpdateDTO>()
        {
            new ()
            {
                Id = 0,
                Index = 2,
                Title = "Title",
                ModelState = ModelState.Updated,
            },
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcode.Tags)
            .WithErrorMessage(expectedError);
    }

    private static UpdateStreetcodeCommand GetValidCreateStreetcodeCommand()
    {
        return new UpdateStreetcodeCommand(new StreetcodeUpdateDTO()
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
            Videos = new List<VideoUpdateDTO>()
            {
                new (),
            },
            Text = new TextUpdateDTO()
            {
                Title = "Taras Shevchenko",
            },
            Subtitles = new List<SubtitleUpdateDTO>()
            {
                new (),
            },
            Tags = new List<StreetcodeTagUpdateDTO>()
            {
                new (),
            },
            ArBlockUrl = "http://streetcode.com.ua/taras-shevchenko",
            Toponyms = new List<StreetcodeToponymCreateUpdateDTO>()
            {
                new (),
            },
            StreetcodeCategoryContents = new List<StreetcodeCategoryContentUpdateDTO>()
            {
                new (),
            },
            Facts = new List<StreetcodeFactUpdateDTO>()
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
        });
    }

    private void SetupRepositoryWrapperReturnsNull()
    {
        _repositoryWrapper.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
            .ReturnsAsync(null as StreetcodeContent);
    }

    private void SetupRepositoryWrapper(int id)
    {
        _repositoryWrapper.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(new StreetcodeContent()
            {
                Id = id,
            });
    }

    private void SetupValidatorMocks()
    {
        _baseStreetcodeValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeCreateUpdateDTO>>()))
            .Returns(new ValidationResult());
        _categoryContentValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeCategoryContentDTO>>()))
            .Returns(new ValidationResult());
        _videoValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<VideoCreateUpdateDTO>>()))
            .Returns(new ValidationResult());
        _baseTextValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<BaseTextDTO>>()))
            .Returns(new ValidationResult());
        _baseFactValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<FactUpdateCreateDto>>()))
            .Returns(new ValidationResult());
        _tagValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreateUpdateTagDTO>>()))
            .Returns(new ValidationResult());
        _baseSubtitleValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<SubtitleCreateUpdateDTO>>()))
            .Returns(new ValidationResult());
    }
}