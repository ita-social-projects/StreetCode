using System.Linq.Expressions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.Validators.News;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.News;

public class NewsTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockFieldsLocalizer;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

    public NewsTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockFieldsLocalizer = new MockFieldNamesLocalizer();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
    }

    [Fact]
    public async void ShouldReturnSuccessResult_WhenNewsIsValid()
    {
        // Arrange
        SetupMockImageRepositoryGetFirstOrDefaultAsync();
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var validNews = GetValidNews();

        // Act
        var result = await validator.ValidateAsync(validNews);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldReturnFail_ImageIdIsZero()
    {
        // Arrange
        SetupMockImageRepositoryGetFirstOrDefaultAsync();
        var news = GetValidNews();
        news.ImageId = 0;
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
        SetupMockImageRepositoryGetFirstOrDefaultAsync();
        var news = GetValidNews();
        news.URL = invalidUrl;
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
        SetupMockImageRepositoryGetFirstOrDefaultAsync();
        var news = GetValidNews();
        SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle(news.Title);
        var baseValidator = new Mock<BaseNewsValidator>(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var createValidator = new CreateNewsValidator(baseValidator.Object, _mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);

        var expectedError = _mockValidationLocalizer["MustBeUnique", _mockFieldsLocalizer["Title"]];

        // Act
        var result = await createValidator.TestValidateAsync(new CreateNewsCommand(news));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.newNews.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnFail_NewsWithSameTextExists()
    {
        // Arrange
        SetupMockImageRepositoryGetFirstOrDefaultAsync();
        var news = GetValidNews();
        SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(news.Text);
        var baseValidator = new Mock<BaseNewsValidator>(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var createValidator = new CreateNewsValidator(baseValidator.Object, _mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);
        var expectedError = _mockValidationLocalizer["MustBeUnique", _mockFieldsLocalizer["Text"]];

        // Act
        var result = await createValidator.TestValidateAsync(new CreateNewsCommand(news));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.newNews.Text)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnFail_CreationDateIsRequired()
    {
        // Arrange
        SetupMockImageRepositoryGetFirstOrDefaultAsync();
        var news = GetValidNews();
        SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(news.Text);
        news.CreationDate = DateTime.MinValue;
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);

        var expectedError = _mockValidationLocalizer["IsRequired", _mockFieldsLocalizer["CreationDate"]];

        // Act
        var result = await validator.TestValidateAsync(news);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreationDate)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnFail_WhenImageDoesNotExist()
    {
        // Arrange
        SetupMockImageRepositoryGetFirstOrDefaultAsync();
        SetupMockImageRepositoryGetFirstOrDefaultAsyncNonExistentImage();
        var news = GetValidNews();
        var validator = new BaseNewsValidator(_mockValidationLocalizer, _mockFieldsLocalizer, _mockRepositoryWrapper.Object);

        var expectedError = _mockValidationLocalizer["ImageDoesntExist", $"{news.ImageId}"];

        // Act
        var result = await validator.TestValidateAsync(news);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
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

    private void SetupMockImageRepositoryGetFirstOrDefaultAsync()
    {
        _mockRepositoryWrapper.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync(new Image { Id = 1 });
    }

    private void SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle(string title)
    {
        _mockRepositoryWrapper.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(() =>
            {
                var newsList = new List<DAL.Entities.News.News>
                {
                    new () { Title = title },
                };

                return newsList.FirstOrDefault();
            });
    }

    private void SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(string text)
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

    private void SetupMockImageRepositoryGetFirstOrDefaultAsyncNonExistentImage()
    {
        _mockRepositoryWrapper.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync((Image)null!);
    }
}