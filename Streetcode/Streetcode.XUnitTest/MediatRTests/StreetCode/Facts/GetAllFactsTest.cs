using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class GetAllFactsTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
    private readonly GetAllFactsHandler _handler;

    public GetAllFactsTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        _handler = new GetAllFactsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WhenFactsExist()
    {
        // Arrange
        var (factsList, factDtoList) = GetFactObjectsLists();
        var request = GetRequest();

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
        VerifyGetAllAsyncAndMockingOperationsExecution(factsList);
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WithCorrectDataType()
    {
        // Arrange
        var (factsList, factDtoList) = GetFactObjectsLists();
        var request = GetRequest();

        MockHelpers.SetupMockFactRepositoryGetAllAsync(_mockRepository, factsList);
        MockHelpers.SetupMockMapper<IEnumerable<FactDto>, List<Fact>>(_mockMapper, factDtoList, factsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<List<FactDto>>();
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WhenFactsNotExist()
    {
        // Arrange
        var (emptyFactsList, emptyFactDtoList) = GetEmptyFactsObjectsLists();
        var request = GetRequest();

        MockHelpers.SetupMockFactRepositoryGetAllAsync(_mockRepository, emptyFactsList);
        MockHelpers.SetupMockMapper(_mockMapper, emptyFactDtoList, emptyFactsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
        result.ValueOrDefault.Should().BeAssignableTo<IEnumerable<FactDto>>();
        VerifyGetAllAsyncAndMockingOperationsExecution(emptyFactsList);
    }

    private static (List<Fact>, List<FactDto>) GetFactObjectsLists()
    {
        var factsList = new List<Fact>()
        {
            new Fact()
            {
                Id = 1,
            },
            new Fact()
            {
                Id = 2,
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

    private static (List<Fact>, List<FactDto>) GetEmptyFactsObjectsLists()
    {
        return (new List<Fact>(), new List<FactDto>());
    }

    private static GetAllFactsQuery GetRequest()
    {
        return new GetAllFactsQuery(UserRole.User);
    }

    private void VerifyGetAllAsyncAndMockingOperationsExecution(List<Fact> factsList)
    {
        _mockRepository.Verify(
            x => x.FactRepository.GetAllAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>, IIncludableQueryable<Fact, object>>>()),
            Times.Once);
        _mockMapper.Verify(x => x.Map<IEnumerable<FactDto>>(factsList), Times.Once);
    }
}
