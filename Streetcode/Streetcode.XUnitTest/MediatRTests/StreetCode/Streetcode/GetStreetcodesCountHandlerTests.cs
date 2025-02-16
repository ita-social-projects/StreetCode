using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetCount;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetStreetcodesCountHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockNoSharedResourceLocalizer _mockLocalizer;
    private readonly GetStreetcodesCountHandler _handler;

    public GetStreetcodesCountHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockLocalizer = new MockNoSharedResourceLocalizer();
        _handler = new GetStreetcodesCountHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mockLocalizer);
    }

    [Fact]
    public async Task Handle_WhenStreetcodesExist_ReturnsCount()
    {
        // Arrange
        var testStreetcodes = GetTestStreetcodes(5);
        SetupRepositoryMock(testStreetcodes);

        var request = new GetStreetcodesCountQuery(OnlyPublished: false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(testStreetcodes.Count, result.Value);
        _repositoryMock.Verify(repo => repo.StreetcodeRepository.GetAllAsync(null, null), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenOnlyPublishedStreetcodesExist_ReturnsPublishedCount()
    {
        // Arrange
        var testStreetcodes = GetTestStreetcodes(5);
        SetupRepositoryMock(testStreetcodes, onlyPublished: true);

        var request = new GetStreetcodesCountQuery(OnlyPublished: true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(testStreetcodes.Count, result.Value);
        _repositoryMock.Verify(
            repo => repo.StreetcodeRepository.GetAllAsync(
            It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoStreetcodesExist_ReturnsError()
    {
        // Arrange
        string expectedErrorKey = "NoStreetcodesExistNow";
        string expectedErrorValue = _mockLocalizer[expectedErrorKey];

        SetupRepositoryMock(new List<StreetcodeContent>());

        var request = new GetStreetcodesCountQuery(OnlyPublished: false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        _loggerMock.Verify(logger => logger.LogError(It.IsAny<GetStreetcodesCountQuery>(), It.IsAny<string>()), Times.Once);
    }

    private static List<StreetcodeContent> GetTestStreetcodes(int count, StreetcodeStatus status = StreetcodeStatus.Published)
    {
        return Enumerable.Range(1, count)
            .Select(i => new StreetcodeContent { Id = i, Status = status })
            .ToList();
    }

    private void SetupRepositoryMock(List<StreetcodeContent> streetcodes, bool onlyPublished = false)
    {
        if (onlyPublished)
        {
            _repositoryMock
                .Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
                .ReturnsAsync(streetcodes);
        }
        else
        {
            _repositoryMock
                .Setup(repo => repo.StreetcodeRepository.GetAllAsync(null, null))
                .ReturnsAsync(streetcodes);
        }
    }
}
