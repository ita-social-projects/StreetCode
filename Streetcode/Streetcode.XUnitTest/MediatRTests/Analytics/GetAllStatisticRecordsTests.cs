using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Analytics;

public class GetAllStatisticRecordsTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly Mock<IStringLocalizer<CannotGetSharedResource>> _stringLocalizerCannotGetMock;
    private readonly Mock<IStringLocalizer<CannotMapSharedResource>> _stringLocalizerCannotMapMock;
    private readonly GetAllStatisticRecordsHandler _handler;

    public GetAllStatisticRecordsTests()
    {
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();
        _stringLocalizerCannotGetMock = new Mock<IStringLocalizer<CannotGetSharedResource>>();
        _stringLocalizerCannotMapMock = new Mock<IStringLocalizer<CannotMapSharedResource>>();

        _handler = new GetAllStatisticRecordsHandler(
            _mapperMock.Object,
            _repositoryWrapperMock.Object,
            _loggerMock.Object,
            _stringLocalizerCannotGetMock.Object,
            _stringLocalizerCannotMapMock.Object);
    }

    [Fact]
    public async Task Handle_RecordsExist_ShouldReturnOrderedRecords()
    {
        // Arrange
        var statisticRecords = GetStatisticRecords();
        var statisticRecordDTOs = GetStatisticRecordDTOs();

        _repositoryWrapperMock.Setup(repo => repo.StatisticRecordRepository.GetAllAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
            .ReturnsAsync(statisticRecords);

        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<StatisticRecordDTO>>(It.IsAny<IEnumerable<StatisticRecord>>()))
            .Returns(statisticRecordDTOs);

        var request = new GetAllStatisticRecordsQuery(UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var orderedRecords = result.Value.ToList();
        Assert.Equal(20, orderedRecords[0].Count);
        Assert.Equal(10, orderedRecords[^1].Count);
        _repositoryWrapperMock.Verify(
            repo => repo.StatisticRecordRepository.GetAllAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<StatisticRecordDTO>>(It.IsAny<IEnumerable<StatisticRecord>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RecordsAreNull_ShouldReturnFail()
    {
        // Arrange
        _repositoryWrapperMock.Setup(
                repo => repo.StatisticRecordRepository.GetAllAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
            .ReturnsAsync((IEnumerable<StatisticRecord>)null!);

        _stringLocalizerCannotGetMock.Setup(localizer => localizer["CannotGetRecords"])
            .Returns(new LocalizedString("CannotGetRecords", "Cannot get statistic records"));

        var request = new GetAllStatisticRecordsQuery(UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Cannot get statistic records", result.Errors[0].Message);
        _loggerMock.Verify(logger => logger.LogError(request, "Cannot get statistic records"), Times.Once);
    }

    [Fact]
    public async Task Handle_MappingFails_ShouldReturnFail()
    {
        // Arrange
        var statisticRecords = GetStatisticRecords();

        _repositoryWrapperMock.Setup(repo => repo.StatisticRecordRepository.GetAllAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
            .ReturnsAsync(statisticRecords);

        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<StatisticRecordDTO>>(It.IsAny<IEnumerable<StatisticRecord>>()))
            .Returns((IEnumerable<StatisticRecordDTO>)null!);

        _stringLocalizerCannotMapMock.Setup(localizer => localizer["CannotMapRecords"])
            .Returns(new LocalizedString("CannotMapRecords", "Cannot map statistic records"));

        var request = new GetAllStatisticRecordsQuery(UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Cannot map statistic records", result.Errors[0].Message);
        _loggerMock.Verify(logger => logger.LogError(request, "Cannot map statistic records"), Times.Once);
    }

    private List<StatisticRecord> GetStatisticRecords()
    {
        return new List<StatisticRecord>
        {
            new () { Id = 1, QrId = 1, Count = 10 },
            new () { Id = 2, QrId = 2, Count = 20 },
            new () { Id = 3, QrId = 3, Count = 15 },
        };
    }

    private List<StatisticRecordDTO> GetStatisticRecordDTOs()
    {
        return new List<StatisticRecordDTO>
        {
            new () { QrId = 1, Count = 10 },
            new () { QrId = 2, Count = 20 },
            new () { QrId = 3, Count = 15 },
        };
    }
}
