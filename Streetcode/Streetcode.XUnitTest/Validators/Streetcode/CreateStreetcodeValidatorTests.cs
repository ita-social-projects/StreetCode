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
using Streetcode.DAL.Entities.Media.Images;
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
        var mockArtValidator = new Mock<ArtCreateUpdateDtoValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);

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
        command.Streetcode.Text = new TextCreateDto()
        {
            Title = string.Empty,
        };
        command.Streetcode.Videos = new List<VideoCreateDto>()
        {
            new VideoCreateDto()
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
    public async Task ShouldReturnError_WhenImageIdsIsEmpty()
    {
        // Arrange
        this.SetupRepositoryWrapperReturnsNull();
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["Images"]];
        var command = this.GetValidCreateStreetcodeCommand();
        command.Streetcode.ImagesIds = new List<int>();

        // Act
        var result = await this.validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcode.ImagesIds)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenImageDoesntExist()
    {
        // Arrange
        this.SetupRepositoryWrapperReturnsNull();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(repositoryWrapper);
        var command = GetValidCreateStreetcodeCommand();
        var invalidImageIds = new List<int>() {10, 20};
        command.Streetcode.ImagesIds = invalidImageIds;


        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        var imageIds = command.Streetcode.ImagesIds.ToList(); 
        for (int i = 0; i < imageIds.Count; i++)
        {
            var imageId = imageIds[i];
            var expectedError = mockValidationLocalizer["ImageDoesntExist", imageId];

            result.ShouldHaveValidationErrorFor($"Streetcode.ImagesIds[{i}]")
                .WithErrorMessage(expectedError);
        }
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
        this.baseStreetcodeValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<StreetcodeCreateUpdateDto>>(), default), Times.AtLeast(1));
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
        this.categoryContentValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<StreetcodeCategoryContentDto>>(), default), Times.AtLeast(1));
        this.videoValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<VideoCreateUpdateDto>>(), default), Times.AtLeast(1));
        this.baseTextValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<BaseTextDto>>(), default), Times.AtLeast(1));
        this.baseFactValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<FactUpdateCreateDto>>(), default), Times.AtLeast(1));
        this.tagValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateUpdateTagDto>>(), default), Times.AtLeast(1));
        this.baseSubtitleValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<SubtitleCreateUpdateDto>>(), default), Times.AtLeast(1));
    }

    private void SetupRepositoryWrapperReturnsNull()
    {
        this.repositoryWrapper.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
            .ReturnsAsync(null as StreetcodeContent);

        this.repositoryWrapper.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync(new Image { Id = 8 });
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

        this.repositoryWrapper.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync(new Image { Id = id });
    }

    private void SetupValidatorMocks()
    {
        this.baseStreetcodeValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeCreateUpdateDto>>()))
            .Returns(new ValidationResult());
        this.categoryContentValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<StreetcodeCategoryContentDto>>()))
            .Returns(new ValidationResult());
        this.videoValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<VideoCreateUpdateDto>>()))
            .Returns(new ValidationResult());
        this.baseTextValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<BaseTextDto>>()))
            .Returns(new ValidationResult());
        this.baseFactValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<FactUpdateCreateDto>>()))
            .Returns(new ValidationResult());
        this.tagValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreateUpdateTagDto>>()))
            .Returns(new ValidationResult());
        this.baseSubtitleValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<SubtitleCreateUpdateDto>>()))
            .Returns(new ValidationResult());
    }

    private CreateStreetcodeCommand GetValidCreateStreetcodeCommand()
    {
        return new CreateStreetcodeCommand(new StreetcodeCreateDto()
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
            Videos = new List<VideoCreateDto>()
            {
                new (),
            },
            Text = new TextCreateDto()
            {
                Title = "Taras Shevchenko",
            },
            Subtitles = new List<SubtitleCreateDto>()
            {
                new (),
            },
            Tags = new List<StreetcodeTagDto>()
            {
                new (),
            },
            ARBlockURL = "http://streetcode.com.ua/taras-shevchenko",
            Toponyms = new List<StreetcodeToponymCreateUpdateDto>()
            {
                new (),
            },
            StreetcodeCategoryContents = new List<CategoryContentCreateDto>()
            {
                new (),
            },
            Facts = new List<StreetcodeFactCreateDto>()
            {
                new (),
            },
            TimelineItems = new List<TimelineItemCreateUpdateDto>()
            {
                new (),
            },
            ImagesIds = new List<int>()
            {
                8,
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
            StreetcodeArtSlides = new List<StreetcodeArtSlideCreateUpdateDto>()
            {
                new (),
            },
            Arts = new List<ArtCreateUpdateDto>()
            {
                new (),
            },
        });
    }
}