using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetStreetcodeByTransliterationUrlHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly StreetcodeContent? _nullValue = null;
    private readonly StreetcodeDTO? _nullValueDto = null;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

    public GetStreetcodeByTransliterationUrlHandlerTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepo = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Theory]
    [InlineData("some")]
    public async Task ExistingUrl(string url)
    {
        // Arrange
        SetupMapper(url);
        SetupRepository(url);

        var handler = new GetStreetcodeByTransliterationUrlHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetStreetcodeByTransliterationUrlQuery(url, UserRole.User), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.TransliterationUrl, url));
    }

    [Theory]
    [InlineData("some")]
    public async Task NotExistingId(string url)
    {
        // Arrange
        _mockRepo.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>())).ReturnsAsync(_nullValue);

        _mockMapper.Setup(x => x.Map<StreetcodeDTO?>(It.IsAny<StreetcodeContent>())).Returns(_nullValueDto);

        var expectedError = $"Cannot find streetcode by transliteration url: {url}";
        _mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
            .Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is string url)
                {
                    return new LocalizedString(key, $"Cannot find streetcode by transliteration url: {url}");
                }

                return new LocalizedString(key, "Cannot find any streetcode with unknown transliteration url");
            });

        var handler = new GetStreetcodeByTransliterationUrlHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetStreetcodeByTransliterationUrlQuery(url, UserRole.User), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors[0].Message));
    }

    [Theory]
    [InlineData("some")]
    public async Task CorrectType(string url)
    {
        // Arrange
        SetupMapper(url);
        SetupRepository(url);

        var handler = new GetStreetcodeByTransliterationUrlHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetStreetcodeByTransliterationUrlQuery(url, UserRole.User), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result.ValueOrDefault),
            () => Assert.IsType<StreetcodeDTO>(result.ValueOrDefault));
    }

    private void SetupRepository(string url)
    {
        _mockRepo
            .Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(new StreetcodeContent() { TransliterationUrl = url });
        _mockRepo
            .Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
                    IIncludableQueryable<StreetcodeTagIndex, object>>>()))
            .ReturnsAsync(new List<StreetcodeTagIndex>());
    }

    private void SetupMapper(string url)
    {
        _mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<StreetcodeContent>())).Returns(new StreetcodeDTO() { TransliterationUrl = url });
    }
}