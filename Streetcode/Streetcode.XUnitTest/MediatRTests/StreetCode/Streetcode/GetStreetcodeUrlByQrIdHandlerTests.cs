using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetUrlByQrId;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetStreetcodeUrlByQrIdHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly GetStreetcodeUrlByQrIdHandler _handler;

    public GetStreetcodeUrlByQrIdHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();

        _handler = new GetStreetcodeUrlByQrIdHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mockCannotFindLocalizer);
    }

    [Fact]
    public async Task Handle_WhenRecordExists_ReturnsStreetcodeUrl()
    {
        // Arrange
        var request = new GetStreetcodeUrlByQrIdQuery(qrId: 10);
        var testStatisticRecord = new StatisticRecord
        {
            QrId = request.qrId,
            StreetcodeCoordinate = new StreetcodeCoordinate { StreetcodeId = 1 },
        };
        var testStreetcode = new StreetcodeContent
        {
            Id = 1,
            TransliterationUrl = "test-url",
        };

        SetupRepositoryMock(testStatisticRecord, testStreetcode);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(testStreetcode.TransliterationUrl, result.Value);
    }

    [Fact]
    public async Task Handle_WhenRecordDoesNotExist_ReturnsError()
    {
        // Arrange
        var request = new GetStreetcodeUrlByQrIdQuery(qrId: 10);
        string expectedErrorKey = "CannotFindRecordWithQrId";
        string expectedErrorValue = _mockCannotFindLocalizer[expectedErrorKey];

        SetupRepositoryMock(null, new StreetcodeContent());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        _loggerMock.Verify(logger => logger.LogError(request, expectedErrorValue), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenStreetcodeDoesNotExist_ReturnsError()
    {
        // Arrange
        var request = new GetStreetcodeUrlByQrIdQuery(qrId: 10);
        var testStatisticRecord = new StatisticRecord
        {
            QrId = request.qrId,
            StreetcodeCoordinate = new StreetcodeCoordinate { StreetcodeId = 1 },
        };
        string expectedErrorKey = "CannotFindStreetcodeById";
        string expectedErrorValue = _mockCannotFindLocalizer[expectedErrorKey];

        SetupRepositoryMock(testStatisticRecord, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        _loggerMock.Verify(logger => logger.LogError(request, expectedErrorValue), Times.Once);
    }

    private void SetupRepositoryMock(StatisticRecord? statisticRecord, StreetcodeContent? streetcode)
    {
        _repositoryMock.Setup(repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
            It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
            .ReturnsAsync(statisticRecord);

        _repositoryMock.Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(streetcode);
    }
}
