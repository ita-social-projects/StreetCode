using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetByQrId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Analytics
{
    public class GetStatisticRecordByQrIdTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _stringLocalizerCannotFindMock;
        private readonly Mock<IStringLocalizer<CannotMapSharedResource>> _stringLocalizerCannotMapMock;
        private readonly GetStatisticRecordByQrIdHandler _handler;

        public GetStatisticRecordByQrIdTests()
        {
            _repositoryMock = new Mock<IRepositoryWrapper>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILoggerService>();
            _stringLocalizerCannotFindMock = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            _stringLocalizerCannotMapMock = new Mock<IStringLocalizer<CannotMapSharedResource>>();

            _handler = new GetStatisticRecordByQrIdHandler(
                _mapperMock.Object,
                _repositoryMock.Object,
                _loggerMock.Object,
                _stringLocalizerCannotMapMock.Object,
                _stringLocalizerCannotFindMock.Object);
        }

        [Fact]
        public async Task Handle_RecordExists_ShouldReturnRecord()
        {
            // Arrange
            var qrId = 1;
            var statisticRecord = GetStatisticRecord(qrId);
            var statisticRecordDto = GetStatisticRecordDtOs(qrId);

            _repositoryMock.Setup(repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                    It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
                .ReturnsAsync(statisticRecord);

            _mapperMock.Setup(mapper => mapper.Map<StatisticRecordDTO>(statisticRecord))
                .Returns(statisticRecordDto);

            var request = new GetStatisticRecordByQrIdQuery(qrId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(qrId, result.Value.QrId);
            _repositoryMock.Verify(
                repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<StatisticRecordDTO>(It.IsAny<StatisticRecord>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RecordNotFound_ShouldReturnFail()
        {
            // Arrange
            var qrId = -1;

            _repositoryMock.Setup(repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                    It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
                .ReturnsAsync((StatisticRecord)null!);

            _stringLocalizerCannotFindMock.Setup(localizer => localizer["CannotFindRecordWithQrId"])
                .Returns(new LocalizedString("CannotFindRecordWithQrId", "Cannot find record with this QR ID"));

            var request = new GetStatisticRecordByQrIdQuery(qrId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal("Cannot find record with this QR ID", result.Errors[0].Message);
            _loggerMock.Verify(logger => logger.LogError(request, "Cannot find record with this QR ID"), Times.Once);
        }

        [Fact]
        public async Task Handle_MappingFails_ShouldReturnFail()
        {
            // Arrange
            var qrId = 1;
            var statisticRecord = GetStatisticRecord(qrId);

            _repositoryMock.Setup(repo => repo.StatisticRecordRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                    It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
                .ReturnsAsync(statisticRecord);

            _mapperMock.Setup(mapper => mapper.Map<StatisticRecordDTO>(It.IsAny<StatisticRecord>()))
                .Returns((StatisticRecordDTO)null!);

            _stringLocalizerCannotMapMock.Setup(localizer => localizer["CannotMapRecord"])
                .Returns(new LocalizedString("CannotMapRecord", "Cannot map the record"));

            var request = new GetStatisticRecordByQrIdQuery(qrId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal("Cannot map the record", result.Errors[0].Message);
            _loggerMock.Verify(logger => logger.LogError(request, "Cannot map the record"), Times.Once);
        }

        private static StatisticRecord GetStatisticRecord(int qrId)
        {
            return new StatisticRecord
            {
                Id = 1,
                QrId = qrId,
                Count = 10,
                StreetcodeCoordinate = new StreetcodeCoordinate { StreetcodeId = 1 },
            };
        }

        private static StatisticRecordDTO GetStatisticRecordDtOs(int qrId)
        {
            return new StatisticRecordDTO
            {
                QrId = qrId,
                Count = 10,
            };
        }
    }
}