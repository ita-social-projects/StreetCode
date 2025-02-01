using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.UpdateCount;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Analytics
{
    public class UpdateCountStatisticRecordTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _stringLocalizerCannotFindMock;
        private readonly Mock<IStringLocalizer<CannotSaveSharedResource>> _stringLocalizerCannotSaveMock;
        private readonly UpdateCountStatisticRecordHandler _handler;

        public UpdateCountStatisticRecordTests()
        {
            _repositoryMock = new Mock<IRepositoryWrapper>();
            _loggerMock = new Mock<ILoggerService>();
            _stringLocalizerCannotFindMock = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            _stringLocalizerCannotSaveMock = new Mock<IStringLocalizer<CannotSaveSharedResource>>();

            _handler = new UpdateCountStatisticRecordHandler(
                _repositoryMock.Object,
                _loggerMock.Object,
                _stringLocalizerCannotSaveMock.Object,
                _stringLocalizerCannotFindMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldIncrementCount_WhenRecordExists()
        {
            // Arrange
            var qrId = 1;
            var statisticRecord = GetStatisticRecord(qrId);

            _repositoryMock.Setup(repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                    It.IsAny<System.Linq.Expressions.Expression<System.Func<StatisticRecord, bool>>>(),
                    null))
                .ReturnsAsync(statisticRecord);

            _repositoryMock.Setup(repo => repo.StatisticRecordRepository.Update(It.IsAny<StatisticRecord>()));

            _repositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            var request = new UpdateCountStatisticRecordCommand(qrId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(6, statisticRecord.Count);
            _repositoryMock.Verify(repo => repo.StatisticRecordRepository.Update(statisticRecord), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenRecordNotFound()
        {
            // Arrange
            var qrId = 1;

            _repositoryMock.Setup(repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                    It.IsAny<System.Linq.Expressions.Expression<System.Func<StatisticRecord, bool>>>(),
                    null))
                .ReturnsAsync((StatisticRecord)null!);

            _stringLocalizerCannotFindMock.Setup(localizer => localizer["CannotFindRecordWithQrId"])
                .Returns(new LocalizedString("CannotFindRecordWithQrId", "Cannot find record with this QR ID"));

            var request = new UpdateCountStatisticRecordCommand(qrId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal("Cannot find record with this QR ID", result.Errors[0].Message);
            _loggerMock.Verify(logger => logger.LogError(request, "Cannot find record with this QR ID"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenSaveFails()
        {
            // Arrange
            var qrId = 1;
            var statisticRecord = GetStatisticRecord(qrId);

            _repositoryMock.Setup(repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                    It.IsAny<System.Linq.Expressions.Expression<System.Func<StatisticRecord, bool>>>(),
                    null))
                .ReturnsAsync(statisticRecord);

            _repositoryMock.Setup(repo => repo.StatisticRecordRepository.Update(It.IsAny<StatisticRecord>()));

            _repositoryMock.Setup(repo => repo.SaveChanges()).Returns(0);

            _stringLocalizerCannotSaveMock.Setup(localizer => localizer["CannotSaveTheData"])
                .Returns(new LocalizedString("CannotSaveTheData", "Cannot save the data"));

            var request = new UpdateCountStatisticRecordCommand(qrId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal("Cannot save the data", result.Errors[0].Message);
            _loggerMock.Verify(logger => logger.LogError(request, "Cannot save the data"), Times.Once);
        }

        private StatisticRecord GetStatisticRecord(int qrId)
        {
            return new StatisticRecord
            {
                Id = 1,
                QrId = qrId,
                Count = 5,
            };
        }
    }
}