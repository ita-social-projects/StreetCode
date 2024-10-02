using System.Linq.Expressions;
using AutoMapper.Configuration;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Serilog.Enrichers;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
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

public class CreateStreetcodeValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly Mock<IRepositoryWrapper> repositoryWrapper;
    private readonly Mock<BaseStreetcodeValidator> baseStreetcodeValidator;
    private readonly Mock<BaseSubtitleValidator> baseSubtitleValidator;
    private readonly Mock<BaseTextValidator> baseTextValidator;
    private readonly Mock<BaseTagValidator> tagValidator;
    private readonly Mock<BaseFactValidator> baseFactValidator;
    private readonly Mock<BaseVideoValidator> videoValidator;
    private readonly Mock<BaseCategoryContentValidator> categoryContentValidator;
    
    private readonly CreateStreetcodeValidator validator;

    public CreateStreetcodeValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.categoryContentValidator = new Mock<BaseCategoryContentValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        this.videoValidator = new Mock<BaseVideoValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        this.baseTextValidator = new Mock<BaseTextValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        this.baseFactValidator = new Mock<BaseFactValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        this.tagValidator = new Mock<BaseTagValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        this.baseSubtitleValidator = new Mock<BaseSubtitleValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        this.repositoryWrapper = new Mock<IRepositoryWrapper>();
        var mockToponymValidator = new Mock<StreetcodeToponymValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var mockContextValidator = new Mock<HistoricalContextValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var mockTimelineItemValidator = new Mock<TimelineItemValidator>(mockContextValidator.Object, this.mockValidationLocalizer, this.mockNamesLocalizer);
        var mockartSlideValidator = new Mock<StreetcodeArtSlideValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        var mockimageDetailsValidator = new Mock<ImageDetailsValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer, this.repositoryWrapper.Object);
        var mockArtValidator = new Mock<ArtCreateUpdateDTOValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);

        this.baseStreetcodeValidator = new Mock<BaseStreetcodeValidator>(
            mockToponymValidator.Object,
            mockTimelineItemValidator.Object,
            mockimageDetailsValidator.Object,
            mockartSlideValidator.Object,
            mockArtValidator.Object,
            this.mockValidationLocalizer,
            this.mockNamesLocalizer);

        this.validator = new CreateStreetcodeValidator(
            this.repositoryWrapper.Object,
            this.baseStreetcodeValidator.Object,
            this.baseTextValidator.Object,
            this.baseSubtitleValidator.Object,
            this.tagValidator.Object,
            this.baseFactValidator.Object,
            this.videoValidator.Object,
            this.categoryContentValidator.Object,
            this.mockValidationLocalizer,
            this.mockNamesLocalizer);
    }

    [Fact]
    public async Task ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        this.SetupRepositoryWrapperReturnsNull();
        var command = this.GetValidCreateStreetcodeCommand();

        // Act
        var result = await this.validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldReturnError_WhenIndexIsNotUnique()
    {
        // Arrange
        this.SetupRepositoryWrapper(1);
        var expectedError = this.mockValidationLocalizer["MustBeUnique", this.mockNamesLocalizer["Index"]];
        var command = this.GetValidCreateStreetcodeCommand();

        // Act
        var result = await this.validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcode.Index)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("testcom")]
    [InlineData("////test/com")]
    [InlineData("ftp://test.com")]
    public async Task ShouldReturnError_WhenArUrlIsInvalid(string invalidUrl)
    {
        // Arrange
        this.SetupRepositoryWrapperReturnsNull();
        var expectedError = this.mockValidationLocalizer["ValidUrl", this.mockNamesLocalizer["ARBlockURL"]];
        var command = this.GetValidCreateStreetcodeCommand();
        command.Streetcode.ARBlockURL = invalidUrl;

        // Act
        var result = await this.validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcode.ARBlockURL)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnError_WhenExistsVideosWithoutTitle()
    {
        // Arrange
        this.SetupRepositoryWrapperReturnsNull();
        var expectedError = this.mockValidationLocalizer["CannotBeEmptyWithCondition", this.mockNamesLocalizer["Title"], this.mockNamesLocalizer["Video"]];
        var command = this.GetValidCreateStreetcodeCommand();
        command.Streetcode.Text = new TextCreateDTO()
        {
            Title = string.Empty,
        };
        command.Streetcode.Videos = new List<VideoCreateDTO>()
        {
            new VideoCreateDTO()
            {
                Url = "uuuurl",
            },
        };

        // Act
        var result = await this.validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcode.Text!.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldCallBaseValidator_WhenValidated()
    {
        // Arrange
        this.SetupRepositoryWrapperReturnsNull();
        this.SetupValidatorMocks();
        var command = this.GetValidCreateStreetcodeCommand();

        // Act
        var result = await this.validator.TestValidateAsync(command);

        // Assert
        this.baseStreetcodeValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<StreetcodeCreateUpdateDTO>>(), default), Times.AtLeast(1));
    }

    [Fact]
    public async Task ShouldCallChildValidators_WhenValidated()
    {
        // Arrange
        this.SetupRepositoryWrapperReturnsNull();
        this.SetupValidatorMocks();
        var command = this.GetValidCreateStreetcodeCommand();

        // Act
        var result = await this.validator.TestValidateAsync(command);

        // Assert
        this.categoryContentValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<StreetcodeCategoryContentDTO>>(), default), Times.AtLeast(1));
        this.videoValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<VideoCreateUpdateDTO>>(), default), Times.AtLeast(1));
        this.baseTextValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<BaseTextDTO>>(), default), Times.AtLeast(1));
        this.baseFactValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<FactUpdateCreateDto>>(), default), Times.AtLeast(1));
        this.tagValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateUpdateTagDTO>>(), default), Times.AtLeast(1));
        this.baseSubtitleValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<SubtitleCreateUpdateDTO>>(), default), Times.AtLeast(1));
    }

    private void SetupRepositoryWrapperReturnsNull()
    {
        this.repositoryWrapper.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
            .ReturnsAsync(null as StreetcodeContent);
    }

    private void SetupRepositoryWrapper(int id)
    {
        this.repositoryWrapper.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(new StreetcodeContent()
            {
                Id = id,
            });
    }

    private void SetupValidatorMocks()
    {
        this.baseStreetcodeValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeCreateUpdateDTO>>()))
            .Returns(new ValidationResult());
        this.categoryContentValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeCategoryContentDTO>>()))
            .Returns(new ValidationResult());
        this.videoValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<VideoCreateUpdateDTO>>()))
            .Returns(new ValidationResult());
        this.baseTextValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<BaseTextDTO>>()))
            .Returns(new ValidationResult());
        this.baseFactValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<FactUpdateCreateDto>>()))
            .Returns(new ValidationResult());
        this.tagValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreateUpdateTagDTO>>()))
            .Returns(new ValidationResult());
        this.baseSubtitleValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<SubtitleCreateUpdateDTO>>()))
            .Returns(new ValidationResult());
    }

    private CreateStreetcodeCommand GetValidCreateStreetcodeCommand()
    {
        return new CreateStreetcodeCommand(new StreetcodeCreateDTO()
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
            Videos = new List<VideoCreateDTO>()
            {
                new (),
            },
            Text = new TextCreateDTO()
            {
                Title = "Taras Shevchenko",
            },
            Subtitles = new List<SubtitleCreateDTO>()
            {
                new (),
            },
            Tags = new List<StreetcodeTagDTO>()
            {
                new (),
            },
            ARBlockURL = "http://streetcode.com.ua/taras-shevchenko",
            Toponyms = new List<StreetcodeToponymCreateUpdateDTO>()
            {
                new (),
            },
            StreetcodeCategoryContents = new List<CategoryContentCreateDTO>()
            {
                new (),
            },
            Facts = new List<StreetcodeFactCreateDTO>()
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
}