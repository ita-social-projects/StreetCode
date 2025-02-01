using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable;
using Moq;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.ExistByQrId;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Analytics;
public class ExistStatisticRecordByQrIdTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly ExistStatisticRecordByQrIdHandler _handler;

    public ExistStatisticRecordByQrIdTests()
    {
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();

        _handler = new ExistStatisticRecordByQrIdHandler(_repositoryWrapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenRecordExists()
    {
        // Arrange
        var qrId = 1;
        var statisticRecords = new List<StatisticRecord>
        {
            new () { QrId = qrId },
        };

        _repositoryWrapperMock.Setup(repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
            .ReturnsAsync(statisticRecords[0]);

        var request = new ExistStatisticRecordByQrIdCommand(qrId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
        _repositoryWrapperMock.Verify(
            repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenRecordDoesNotExist()
    {
        // Arrange
        var qrId = -1;

        _repositoryWrapperMock.Setup(
                repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
            .ReturnsAsync((StatisticRecord)null!);

        var request = new ExistStatisticRecordByQrIdCommand(qrId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
        _repositoryWrapperMock.Verify(
            repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
            It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()), Times.Once);
    }
}