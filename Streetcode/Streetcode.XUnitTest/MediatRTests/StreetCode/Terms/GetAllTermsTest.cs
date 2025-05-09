using AutoMapper;
using FluentAssertions;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;
using Streetcode.BLL.MediatR.Streetcode.Term.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

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
        var terms = GetTermEntities(2);
        var termDtos = GetTermDtos(2);
        var request = new GetAllTermsQuery();

        _mockRepository.Setup(r => r.TermRepository.GetAllAsync(
                It.IsAny<Expression<Func<Term, bool>>>(),
                It.IsAny<Func<IQueryable<Term>, IIncludableQueryable<Term, object>>>()))
            .ReturnsAsync(terms);

        _mockMapper.Setup(m => m.Map<IEnumerable<TermDto>>(terms))
            .Returns(termDtos);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Terms.Should().HaveCount(2);
        result.Value.Terms.Select(t => t.Id).Should().ContainInOrder(0, 1);

        VerifyGetAllAndMapping(terms);
    }

    [Fact]
    public async Task ShouldReturnEmpty_WhenNoTermsExist()
    {
        // Arrange
        var terms = new List<Term>();
        var termDtos = new List<TermDto>();
        var request = new GetAllTermsQuery();

        _mockRepository.Setup(r => r.TermRepository.GetAllAsync(
                It.IsAny<Expression<Func<Term, bool>>>(),
                It.IsAny<Func<IQueryable<Term>, IIncludableQueryable<Term, object>>>()))
            .ReturnsAsync(terms);

        _mockMapper.Setup(m => m.Map<IEnumerable<TermDto>>(terms))
            .Returns(termDtos);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Terms.Should().BeEmpty();

        VerifyGetAllAndMapping(terms);
    }

    [Fact]
    public async Task ShouldReturnCorrectDataType()
    {
        // Arrange
        var terms = GetTermEntities(2);
        var termDtos = GetTermDtos(2);
        var request = new GetAllTermsQuery();

        _mockRepository.Setup(r => r.TermRepository.GetAllAsync(
                It.IsAny<Expression<Func<Term, bool>>>(),
                It.IsAny<Func<IQueryable<Term>, IIncludableQueryable<Term, object>>>()))
            .ReturnsAsync(terms);

        _mockMapper.Setup(m => m.Map<IEnumerable<TermDto>>(terms))
            .Returns(termDtos);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Terms.Should().BeOfType<List<TermDto>>();

        VerifyGetAllAndMapping(terms);
    }

    private static List<Term> GetTermEntities(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Term { Id = i })
            .ToList();
    }

    private static List<TermDto> GetTermDtos(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new TermDto { Id = i })
            .ToList();
    }

    private void VerifyGetAllAndMapping(IEnumerable<Term> terms)
    {
        _mockRepository.Verify(r => r.TermRepository.GetAllAsync(
            It.IsAny<Expression<Func<Term, bool>>>(),
            It.IsAny<Func<IQueryable<Term>, IIncludableQueryable<Term, object>>>()), Times.Once);

        _mockMapper.Verify(m => m.Map<IEnumerable<TermDto>>(terms), Times.Once);
    }
}