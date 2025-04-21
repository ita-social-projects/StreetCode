using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Term.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
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
        var (termsList, termDtoList) = GetTermObjectsLists();
        var request = GetRequest();

        MockHelpers.SetupMockTermRepositoryGetAllAsync(_mockRepository, termsList);
        MockHelpers.SetupMockMapper<IEnumerable<TermDTO>, List<Term>>(_mockMapper, termDtoList, termsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var value = result.Value.Should().BeAssignableTo<GetAllTermsResponseDto>().Which;

        var terms = value.Terms;

        terms.Should().SatisfyRespectively(
            first => first.Id.Should().Be(termsList[0].Id),
            second => second.Id.Should().Be(termsList[1].Id));

        terms.Should().HaveCount(2);

        VerifyGetAllAsyncAndMockingOperationsExecution(termsList);
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WithCorrectDataType()
    {
        // Arrange
        var (termsList, termDtoList) = GetTermObjectsLists();
        var request = GetRequest();

        MockHelpers.SetupMockTermRepositoryGetAllAsync(_mockRepository, termsList);
        MockHelpers.SetupMockMapper<IEnumerable<TermDTO>, List<Term>>(_mockMapper, termDtoList, termsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<GetAllTermsResponseDto>();
        result.ValueOrDefault.Terms.Should().BeAssignableTo<List<TermDTO>>();
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WhenTermsNotExist()
    {
        // Arrange
        var (termsList, termDtoList) = GetEmptyTermObjectsLists();
        var request = GetRequest();

        MockHelpers.SetupMockTermRepositoryGetAllAsync(_mockRepository, termsList);
        MockHelpers.SetupMockMapper(_mockMapper, termDtoList, termsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var value = result.Value.Should().BeAssignableTo<GetAllTermsResponseDto>().Which;

        value.Terms.Should().BeEmpty();

        result.ValueOrDefault.Should().BeAssignableTo<GetAllTermsResponseDto>();

        VerifyGetAllAsyncAndMockingOperationsExecution(termsList);
    }

    private static (List<Term>, List<TermDTO>) GetTermObjectsLists()
    {
        var termsList = new List<Term>()
        {
            new Term()
            {
                Id = 1,
            },
            new Term()
            {
                Id = 2,
            },
        };
        var termDtoList = new List<TermDTO>()
        {
            new TermDTO()
            {
                Id = termsList[0].Id,
            },
            new TermDTO()
            {
                Id = termsList[1].Id,
            },
        };

        return (termsList, termDtoList);
    }

    private static (List<Term>, List<TermDTO>) GetEmptyTermObjectsLists()
    {
        return (new List<Term>(), new List<TermDTO>());
    }

    private static GetAllTermsQuery GetRequest()
    {
        return new GetAllTermsQuery();
    }

    private void VerifyGetAllAsyncAndMockingOperationsExecution(List<Term> termsList)
    {
        _mockRepository.Verify(
            x => x.TermRepository.GetAllAsync(
                It.IsAny<Expression<Func<Term, bool>>>(),
                It.IsAny<Func<IQueryable<Term>, IIncludableQueryable<Term, object>>>()),
            Times.Once);
        _mockMapper.Verify(x => x.Map<IEnumerable<TermDTO>>(termsList), Times.Once);
    }
}
