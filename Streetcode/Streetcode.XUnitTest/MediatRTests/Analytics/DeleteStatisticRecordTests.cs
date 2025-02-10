using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Analytics;

public class DeleteStatisticRecordTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _localizerCannotFindMock;
    private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _localizerFailedToDeleteMock;
    private readonly DeleteStatisticRecordHandler _handler;

    public DeleteStatisticRecordTests()
    {
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _loggerMock = new Mock<ILoggerService>();
        _localizerCannotFindMock = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        _localizerFailedToDeleteMock = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();

        _handler = new DeleteStatisticRecordHandler(
            _repositoryWrapperMock.Object,
            _loggerMock.Object,
            _localizerCannotFindMock.Object,
            _localizerFailedToDeleteMock.Object);
    }

    [Fact]
    public async Task Handle_RecordIsDeletedSuccessfully_ShouldReturnSuccess()
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

        _repositoryWrapperMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

        var request = new DeleteStatisticRecordCommand(qrId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Unit.Value, result.Value);
        _repositoryWrapperMock.Verify(repo => repo.StatisticRecordRepository.Delete(It.IsAny<StatisticRecord>()), Times.Once);
        _repositoryWrapperMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_RecordIsNotFound_ShouldReturnError()
    {
        // Arrange
        var qrId = -1;

        _repositoryWrapperMock.Setup(repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
            .ReturnsAsync((StatisticRecord)null!);

        _localizerCannotFindMock.Setup(loc => loc["CannotFindRecordWithQrId"]).Returns(new LocalizedString("CannotFindRecordWithQrId", "Record not found"));

        var request = new DeleteStatisticRecordCommand(qrId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Record not found", result.Errors[0].Message);
        _repositoryWrapperMock.Verify(repo => repo.StatisticRecordRepository.Delete(It.IsAny<StatisticRecord>()), Times.Never);
        _repositoryWrapperMock.Verify(repo => repo.SaveChanges(), Times.Never);
    }

    [Fact]
    public async Task Handle_DeleteFails_ShouldReturnError()
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

        _repositoryWrapperMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(0);

        _localizerFailedToDeleteMock.Setup(loc => loc["FailedToDeleteTheRecord"]).Returns(new LocalizedString("FailedToDeleteTheRecord", "Failed to delete record"));

        var request = new DeleteStatisticRecordCommand(qrId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Failed to delete record", result.Errors[0].Message);
        _repositoryWrapperMock.Verify(repo => repo.StatisticRecordRepository.Delete(It.IsAny<StatisticRecord>()), Times.Once);
        _repositoryWrapperMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}
