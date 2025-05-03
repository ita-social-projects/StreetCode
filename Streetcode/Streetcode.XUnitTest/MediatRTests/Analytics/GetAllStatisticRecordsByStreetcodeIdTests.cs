using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAllByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Analytics;

public class GetAllStatisticRecordsByStreetcodeIdTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _stringLocalizerCannotFindMock;
    private readonly GetAllStatisticRecordsByStreetcodeIdHandler _handler;

    public GetAllStatisticRecordsByStreetcodeIdTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();
        _stringLocalizerCannotFindMock = new Mock<IStringLocalizer<CannotFindSharedResource>>();

        _handler = new GetAllStatisticRecordsByStreetcodeIdHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _stringLocalizerCannotFindMock.Object);
    }

    [Fact]
    public async Task Handle_RecordsExistAndUserHasAccess_ShouldReturnRecords()
    {
        // Arrange
        int streetcodeId = 1;
        var statisticRecords = GetStatisticRecords(streetcodeId);
        var statisticRecordDtos = GetStatisticRecordDtos();

        SetupMockRepository(statisticRecords, new List<StreetcodeContent>() { new () { Id = streetcodeId } });

        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<StatisticRecordDTO>>(It.IsAny<IEnumerable<StatisticRecord>>()))
            .Returns(statisticRecordDtos);

        var request = new GetAllStatisticRecordsByStreetcodeIdQuery(streetcodeId, UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var records = result.Value.ToList();
        Assert.Equal(2, records.Count);
        Assert.Equal(1, records[0].QrId);
        Assert.Equal(2, records[1].QrId);
        _repositoryMock.Verify(
            repo => repo.StatisticRecordRepository.GetAllAsync(
            It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
            It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<StatisticRecordDTO>>(It.IsAny<IEnumerable<StatisticRecord>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RecordsExistButUserDoesNotHavaAccess_ShouldReturnRecords()
    {
        // Arrange
        int streetcodeId = 2;

        SetupMockRepository(Enumerable.Empty<StatisticRecord>().AsQueryable().BuildMock(), new List<StreetcodeContent>());

        _stringLocalizerCannotFindMock.Setup(localizer => localizer["CannotFindAnyStreetcodeWithCorrespondingId", streetcodeId])
            .Returns(new LocalizedString("CannotFindAnyStreetcodeWithCorrespondingId", $"Cannot find any streetcode with corresponding id {streetcodeId}"));

        var request = new GetAllStatisticRecordsByStreetcodeIdQuery(streetcodeId, UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal($"Cannot find any streetcode with corresponding id {streetcodeId}", result.Errors[0].Message);
        _loggerMock.Verify(logger => logger.LogError(request, $"Cannot find any streetcode with corresponding id {streetcodeId}"), Times.Once);
    }

    [Fact]
    public async Task Handle_UserHasAccessAndRecordsNotFound_ShouldReturnFail()
    {
        // Arrange
        int streetcodeId = 2;

        SetupMockRepository(Enumerable.Empty<StatisticRecord>().AsQueryable().BuildMock(), new List<StreetcodeContent>() { new () { Id = streetcodeId } });

        _stringLocalizerCannotFindMock.Setup(localizer => localizer["CannotFindRecordWithStreetcodeId", streetcodeId])
            .Returns(new LocalizedString("CannotFindRecordWithStreetcodeId", $"Cannot find records with StreetcodeId {streetcodeId}"));

        var request = new GetAllStatisticRecordsByStreetcodeIdQuery(streetcodeId, UserRole.User);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal($"Cannot find records with StreetcodeId {streetcodeId}", result.Errors[0].Message);
        _loggerMock.Verify(logger => logger.LogError(request, $"Cannot find records with StreetcodeId {streetcodeId}"), Times.Once);
    }

    private void SetupMockRepository(IEnumerable<StatisticRecord> returns, List<StreetcodeContent> streetcodeListUserCanAccess)
    {
        _repositoryMock.Setup(repo => repo.StatisticRecordRepository.GetAllAsync(
                It.IsAny<Expression<Func<StatisticRecord, bool>>>(),
                It.IsAny<Func<IQueryable<StatisticRecord>, IIncludableQueryable<StatisticRecord, object>>>()))
            .ReturnsAsync(returns);

        _repositoryMock.Setup(repo => repo.StreetcodeRepository
                .FindAll(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                        IIncludableQueryable<StreetcodeContent, object>>>()))
            .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);
    }

    private static List<StatisticRecordDTO> GetStatisticRecordDtos()
    {
        return new List<StatisticRecordDTO>
        {
            new () { QrId = 1, Count = 10 },
            new () { QrId = 2, Count = 15 },
        };
    }

    private List<StatisticRecord> GetStatisticRecords(int streetcodeId)
    {
        return new List<StatisticRecord>
        {
            new ()
            {
                Id = 1, QrId = 1, Count = 10,
                StreetcodeCoordinate = new StreetcodeCoordinate { StreetcodeId = streetcodeId },
            },
            new ()
            {
                Id = 2, QrId = 2, Count = 15,
                StreetcodeCoordinate = new StreetcodeCoordinate { StreetcodeId = streetcodeId },
            },
        };
    }
}