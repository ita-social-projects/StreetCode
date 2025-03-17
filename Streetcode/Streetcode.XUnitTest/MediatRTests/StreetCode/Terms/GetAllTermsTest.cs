using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;
using Streetcode.BLL.MediatR.Streetcode.Term.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms;

public class GetAllTermsTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllTermsHandler _handler;

    public GetAllTermsTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllTermsHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WhenTermsExist()
    {
        // Arrange
        const int objectsNumber = 2;
        var (termsPaginated, termDtoList) = GetTermObjects(objectsNumber);
        var request = GetRequest();

        MockHelpers.SetupMockTermRepositoryGetAllPaginated(_mockRepository, termsPaginated);
        MockHelpers.SetupMockMapper<IEnumerable<TermDto>, IEnumerable<Term>>(
            _mockMapper,
            termDtoList,
            termsPaginated.Entities);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var initialTermsList = termsPaginated.Entities.ToList();
        result.IsSuccess.Should().BeTrue();
        result.Value.Terms.Should().SatisfyRespectively(
            first => first.Id.Should().Be(initialTermsList[0].Id),
            second => second.Id.Should().Be(initialTermsList[1].Id));
        result.Value.TotalAmount.Should().Be(objectsNumber);
        VerifyGetAllPaginatedAndMockingOperationsExecution(termsPaginated.Entities);
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WithCorrectDataType()
    {
        // Arrange
        const int objectsNumber = 2;
        var (termsPaginated, termDtoList) = GetTermObjects(objectsNumber);
        var request = GetRequest();

        MockHelpers.SetupMockTermRepositoryGetAllPaginated(_mockRepository, termsPaginated);
        MockHelpers.SetupMockMapper<IEnumerable<TermDto>, IEnumerable<Term>>(
            _mockMapper,
            termDtoList,
            termsPaginated.Entities);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<GetAllTermsDto>();
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WhenTermsNotExist()
    {
        // Arrange
        var (emptyTermsPaginated, emptyTermDtoList) = GetEmptyTermObjects();
        var request = GetRequest();

        MockHelpers.SetupMockTermRepositoryGetAllPaginated(_mockRepository, emptyTermsPaginated);
        MockHelpers.SetupMockMapper(_mockMapper, emptyTermDtoList, emptyTermsPaginated.Entities);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Terms.Should().BeEmpty();
        result.ValueOrDefault.Should().BeAssignableTo<GetAllTermsDto>();
        VerifyGetAllPaginatedAndMockingOperationsExecution(emptyTermsPaginated.Entities);
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WithCorrectPageSize()
    {
        // Arrange
        const ushort pageNumber = 1;
        const ushort pageSize = 2;
        var (termsPaginated, termDtoList) = GetTermObjects(pageSize);
        var request = GetRequest(pageNumber, pageSize);

        MockHelpers.SetupMockTermRepositoryGetAllPaginated(_mockRepository, termsPaginated);
        MockHelpers.SetupMockMapper<IEnumerable<TermDto>, IEnumerable<Term>>(
            _mockMapper,
            termDtoList,
            termsPaginated.Entities);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Terms.Should().NotBeEmpty();
        result.Value.TotalAmount.Should().Be(pageSize);
    }

    [Fact]
    public async Task ShouldGetAllPaginatedSuccessfully_WhenPageNumberIsTooBig()
    {
        // Arrange
        const ushort pageNumber = 99;
        const ushort pageSize = 2;
        var (emptyTermsPaginated, emptyTermDtoList) = GetEmptyTermObjects();
        var request = GetRequest(pageNumber, pageSize);

        MockHelpers.SetupMockTermRepositoryGetAllPaginated(_mockRepository, emptyTermsPaginated);
        MockHelpers.SetupMockMapper(_mockMapper, emptyTermDtoList, emptyTermsPaginated.Entities);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Terms.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public async Task ShouldGetAllPaginatedSuccessfully_WhenPageNumberOrSizeIsZero(ushort pageNumber, ushort pageSize)
    {
        // Arrange
        var (emptyTermsPaginated, emptyTermDtoList) = GetEmptyTermObjects();
        var request = GetRequest(pageNumber, pageSize);

        MockHelpers.SetupMockTermRepositoryGetAllPaginated(_mockRepository, emptyTermsPaginated);
        MockHelpers.SetupMockMapper(_mockMapper, emptyTermDtoList, emptyTermsPaginated.Entities);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Terms.Should().BeEmpty();
    }

    private static (PaginationResponse<Term>, List<TermDto>) GetTermObjects(int count)
    {
        var termsList = Enumerable
            .Range(0, count)
            .Select(i => new Term() { Id = i })
            .ToList();
        var termsPaginated = PaginationResponse<Term>.Create(termsList.AsQueryable());

        var termDtoList = Enumerable
            .Range(0, count)
            .Select(i => new TermDto() { Id = termsList[i].Id })
            .ToList();

        return (termsPaginated, termDtoList);
    }

    private static (PaginationResponse<Term>, List<TermDto>) GetEmptyTermObjects()
    {
        return (PaginationResponse<Term>.Create(new List<Term>().AsQueryable()), new List<TermDto>());
    }

    private static GetAllTermsQuery GetRequest(ushort? page = null, ushort? pageSize = null)
    {
        return new GetAllTermsQuery(page, pageSize);
    }

    private void VerifyGetAllPaginatedAndMockingOperationsExecution(IEnumerable<Term> termsList)
    {
        _mockRepository.Verify(
            x => x.TermRepository.GetAllPaginated(
                It.IsAny<ushort?>(),
                It.IsAny<ushort?>(),
                It.IsAny<Expression<Func<Term, Term>>?>(),
                It.IsAny<Expression<Func<Term, bool>>?>(),
                It.IsAny<Func<IQueryable<Term>, IIncludableQueryable<Term, object>>?>(),
                It.IsAny<Expression<Func<Term, object>>?>(),
                It.IsAny<Expression<Func<Term, object>>?>()),
            Times.Once);
        _mockMapper.Verify(x => x.Map<IEnumerable<TermDto>>(termsList), Times.Once);
    }
}
