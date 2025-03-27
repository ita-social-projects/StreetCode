using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class GetFactsByStreetcodeIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly GetFactByStreetcodeIdHandler _handler;

    public GetFactsByStreetcodeIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _handler = new GetFactByStreetcodeIdHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockCannotFindLocalizer);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByStreetcodeIdSuccessfully_WhenIdExists(int streetcodeId)
    {
        // Arrange
        var (factsList, factDtoList) = GetFactObjectsLists(streetcodeId);
        var request = GetRequest(streetcodeId);

        MockHelpers.SetupMockFactRepositoryGetAllAsync(_mockRepository, factsList);
        MockHelpers.SetupMockMapper<IEnumerable<FactDto>, List<Fact>>(_mockMapper, factDtoList, factsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().SatisfyRespectively(
            first => first.Id.Should().Be(factsList[0].Id),
            second => second.Id.Should().Be(factsList[1].Id));
        result.Value.Should().HaveCount(2);
        _mockMapper.Verify(x => x.Map<IEnumerable<FactDto>>(factsList), Times.Once);
        VerifyGetAllAsyncOperationExecution(request);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByStreetcodeIdSuccessfully_WithCorrectDataType(int streetcodeId)
    {
        // Arrange
        var (factsList, factDtoList) = GetFactObjectsLists(streetcodeId);
        var request = GetRequest(streetcodeId);

        MockHelpers.SetupMockFactRepositoryGetAllAsync(_mockRepository, factsList);
        MockHelpers.SetupMockMapper<IEnumerable<FactDto>, List<Fact>>(_mockMapper, factDtoList, factsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<List<FactDto>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByStreetcodeIdSuccessfully_WhenIdNotExists(int streetcodeId)
    {
        // Arrange
        var emptyFactsList = GetEmptyFactsList();
        var request = GetRequest(streetcodeId);

        MockHelpers.SetupMockFactRepositoryGetAllAsync(_mockRepository, emptyFactsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
        result.ValueOrDefault.Should().BeAssignableTo<IEnumerable<FactDto>>();
        VerifyGetAllAsyncOperationExecution(request);
    }

    private static (List<Fact>, List<FactDto>) GetFactObjectsLists(int streetcodeId)
    {
        var factsList = new List<Fact>()
        {
            new Fact()
            {
                Id = 1,
                StreetcodeId = streetcodeId,
            },
            new Fact()
            {
                Id = 2,
                StreetcodeId = streetcodeId,
            },
        };
        var factDtoList = new List<FactDto>()
        {
            new FactDto()
            {
                Id = factsList[0].Id,
            },
            new FactDto()
            {
                Id = factsList[1].Id,
            },
        };

        return (factsList, factDtoList);
    }

    private static List<Fact> GetEmptyFactsList()
    {
        return new List<Fact>();
    }

    private static GetFactByStreetcodeIdQuery GetRequest(int streetcodeId)
    {
        return new GetFactByStreetcodeIdQuery(streetcodeId);
    }

    private void VerifyGetAllAsyncOperationExecution(GetFactByStreetcodeIdQuery request)
    {
        _mockRepository.Verify(
            x => x.FactRepository.GetAllAsync(
                f => f.StreetcodeId == request.StreetcodeId,
                It.IsAny<Func<IQueryable<Fact>, IIncludableQueryable<Fact, object>>>()),
            Times.Once);
    }
}
