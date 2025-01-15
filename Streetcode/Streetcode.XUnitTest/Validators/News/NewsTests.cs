using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.MediatR.Newss.Update;
using Streetcode.BLL.Validators.News;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace Streetcode.XUnitTest.Validators.News;

public class NewsTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockFieldsLocalizer;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

    public NewsTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockFieldsLocalizer = new MockFieldNamesLocalizer();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
    }

    [Fact]
    public async void ShouldReturnSuccessResult_WhenNewsIsValid()
    {
        // Arrange
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var validNews = GetValidNews();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, validNews.ImageId);
        SetupMockRepositoryGetFirstOrDefaultAsyncWithValidNews();
        // Act
        var result = await validator.ValidateAsync(validNews);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                _testOutputHelper.WriteLine($"Validation failed: {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldReturnFail_ImageIdIsZero()
    {
        // Arrange
        var news = GetValidNews();
        news.ImageId = 0;
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var expectedError = _mockValidationLocalizer["Invalid", _mockFieldsLocalizer["ImageId"]];

        // Act
        var result = await validator.TestValidateAsync(news);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("@")]
    [InlineData("test-url!")]
    [InlineData("test_url")]
    [InlineData("Test-URL")]
    [InlineData("5test-url%")]
    public async Task ShouldReturnFail_UrlIsInvalid(string invalidUrl)
    {
        // Arrange
        var news = GetValidNews();
        news.URL = invalidUrl;
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var expectedError = _mockValidationLocalizer["InvalidNewsUrl"];

        // Act
        var result = await validator.TestValidateAsync(news);

        // Asssert
        result.ShouldHaveValidationErrorFor(x => x.URL)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnFail_NewsWithSameTitleExists()
    {
        // Arrange
        var news = GetValidNews();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle(news.Title);
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);

        var expectedError = _mockValidationLocalizer["MustBeUnique", _mockFieldsLocalizer["Title"]];

        // Act
        var result = await validator.TestValidateAsync(news);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnFail_NewsWithSameTextExists()
    {
        // Arrange
        var news = GetValidNews();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(news.Text);
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var expectedError = _mockValidationLocalizer["MustBeUnique", _mockFieldsLocalizer["Text"]];

        // Act
        var result = await validator.TestValidateAsync(news);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Text)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnFail_WhenImageDoesNotExist()
    {
        // Arrange
        var news = GetValidNews();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(_mockRepositoryWrapper);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);

        var expectedError = _mockValidationLocalizer["ImageDoesntExist", $"{news.ImageId}"];

        // Act
        var result = await validator.TestValidateAsync(news);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenTitleIsEmpty()
    {
        // Arrange
        var validator =
            new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var news = GetValidNews();
        news.Title = string.Empty;
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockFieldsLocalizer["Title"]];

        // Act
        var result = await validator.TestValidateAsync(news);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenTextIsEmpty()
    {
        // Arrange
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var news = GetValidNews();
        news.Text = string.Empty;
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var expectedErrorMessage = _mockValidationLocalizer["CannotBeEmpty", _mockFieldsLocalizer["Text"]];

        // Act
        var result = await validator.TestValidateAsync(news);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Text)
            .WithErrorMessage(expectedErrorMessage);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenCreationDateIsEmpty()
    {
        // Arrange
        var news = GetValidNews();
        news.CreationDate = DateTime.MinValue;
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);

        var expectedError = _mockValidationLocalizer["IsRequired", _mockFieldsLocalizer["CreationDate"]];

        // Act
        var result = await validator.TestValidateAsync(news);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreationDate)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenURLIsEmpty()
    {
        // Arrange
        var validator =
            new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var news = GetValidNews();
        news.URL = string.Empty;
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockFieldsLocalizer["TargetUrl"]];

        // Act
        var result = await validator.TestValidateAsync(news);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.URL)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenTitleExceedsMaxLength()
    {
        // Arrange
        var validator =
            new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var news = GetValidNews();
        news.Title = new string('a', BaseNewsValidator.TitleMaxLength + 1);
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockFieldsLocalizer["Title"], $"{BaseNewsValidator.TitleMaxLength}"];

        // Act
        var result = await validator.TestValidateAsync(news);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnValidationError_WhenURLExceedsMaxLength()
    {
        // Arrange
        var validator =
            new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var news = GetValidNews();
        news.URL = new string('a', BaseNewsValidator.UrlMaxLength + 1);
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, news.ImageId);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var expectedError =
            _mockValidationLocalizer["MaxLength", _mockFieldsLocalizer["TargetUrl"], BaseNewsValidator.UrlMaxLength];

        // Act
        var result = await validator.TestValidateAsync(news);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.URL)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task UpdateNewsValidator_ShouldCallBaseValidator()
    {
        // Arrange
        SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle("Test Title", 1); // The Id will be 2, so the method will return false
        SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText("Test Text", 1); // The Id will be 2, so the method will return false
        SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingUrl("test-url23", 1); // The Id will be 2, so the method will return false
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, 1);
        MockHelpers.SetupMockNewsRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper);
        var baseValidator = new Mock<BaseNewsValidator>(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        baseValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateUpdateNewsDTO>>(), default))
            .ReturnsAsync(new ValidationResult());

        _mockRepositoryWrapper.Setup(x => x.NewsRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(() => default);

        var createValidator = new UpdateNewsValidator(baseValidator.Object);
        var createCommand = new UpdateNewsCommand(new UpdateNewsDTO()
        {
            Title = "Test Title",
            Text = "Test Text", // Text that is not unique
            CreationDate = DateTime.Now,
            ImageId = 1,
            URL = "test-url23",
        });

        // Act
        var result = await createValidator.TestValidateAsync(createCommand);
        foreach (var error in result.Errors)
        {
            _testOutputHelper.WriteLine(error.ToString()); // displaying all the errors in the console
        }

        // Assert
        baseValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateUpdateNewsDTO>>(), default), Times.Once);
    }

    [Fact]
    public async Task CreateNewsValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var baseValidator = new Mock<BaseNewsValidator>(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        baseValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateUpdateNewsDTO>>(), default))
            .ReturnsAsync(new ValidationResult());

        _mockRepositoryWrapper.Setup(x => x.NewsRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(() => default);

        var createValidator = new CreateNewsValidator(baseValidator.Object);
        var createCommand = new CreateNewsCommand(new NewsCreateDTO()
        {
            Title = "Test Title",
            Text = "Test Text", // Text that is not unique
            CreationDate = DateTime.Now,
            ImageId = 1,
            URL = "test-url23",
        });

        // Act
        var result = await createValidator.TestValidateAsync(createCommand);
        foreach (var error in result.Errors)
        {
            _testOutputHelper.WriteLine(error.ToString()); // displaying all the errors in the console
        }

        // Assert
        baseValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateUpdateNewsDTO>>(), default), Times.Once);
    }

    private static NewsCreateDTO GetValidNews()
    {
        return new NewsCreateDTO()
        {
            Title = "Test Title",
            Text = "Test Text",
            ImageId = 3,
            URL = "test-url23",
            CreationDate = DateTime.Now,
        };
    }

    private void SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle(string title)
    {
        _mockRepositoryWrapper.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>,
                    IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(new DAL.Entities.News.News
                {
                    Title = title
                }
            );
    }

    private void SetupMockRepositoryGetFirstOrDefaultAsyncWithValidNews()
    {
        _mockRepositoryWrapper.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>,
                    IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync((DAL.Entities.News.News)null
            
            );
    }

    private void SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText( string text)
    {
        _mockRepositoryWrapper.Setup(x => x.NewsRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(() =>
            {
                var newsList = new List<DAL.Entities.News.News>
                {
                    new () { Text = text },
                };

                return newsList.FirstOrDefault();
            });
    }

    private void SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle(string title, int existingId)
    {
        _mockRepositoryWrapper.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(() =>
            {
                var newsList = new List<DAL.Entities.News.News>
                {
                    // News with the same title, but a different Id
                    new () { Title = title, Id = existingId + 1 },
                };

                return newsList.FirstOrDefault();
            });
    }

    private void SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(string text, int existingId)
    {
        _mockRepositoryWrapper.Setup(x => x.NewsRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(() =>
            {
                var newsList = new List<DAL.Entities.News.News>
                {
                    // News with the same text, but a different Id
                    new () { Text = text, Id = existingId + 1 },
                };

                return newsList.FirstOrDefault();
            });
    }

    private void SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingUrl(string url, int existingId)
    {
        _mockRepositoryWrapper.Setup(x => x.NewsRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(() =>
            {
                var newsList = new List<DAL.Entities.News.News>
                {
                    // News with the same URL, but a different Id
                    new () { URL = url, Id = existingId + 1 },
                };

                return newsList.FirstOrDefault();
            });
    }
}