using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Term.GetById;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms;

public class GetTermByIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly GetTermByIdHandler _handler;

    public GetTermByIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _handler = new GetTermByIdHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockCannotFindLocalizer);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByIdSuccessfully_WhenIdExists(int termId)
    {
        // Arrange
        var (term, termDto) = GetTermObjects(termId);
        var request = GetRequest(termId);

        SetupMockRepository(request, term);
        MockHelpers.SetupMockMapper(_mockMapper, termDto, term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(term.Id);
        _mockRepository.Verify(
            x => x.TermRepository.GetFirstOrDefaultAsync(
                t => t.Id == request.Id,
                It.IsAny<Func<IQueryable<Term>, IIncludableQueryable<Term, object>>>()),
            Times.Once);
        _mockMapper.Verify(x => x.Map<TermDTO>(term), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByIdSuccessfully_WithCorrectDataType(int termId)
    {
        // Arrange
        var (term, termDto) = GetTermObjects(termId);
        var request = GetRequest(termId);

        SetupMockRepository(request, term);
        MockHelpers.SetupMockMapper(_mockMapper, termDto, term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<TermDTO>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByIdFailingly_WhenTermNotFound(int termId)
    {
        // Arrange
        var request = GetRequest(termId);
        var expectedErrorMessage = _mockCannotFindLocalizer["CannotFindAnyTermWithCorrespondingId", request.Id].Value;

        SetupMockRepository(request, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (Term, TermDTO) GetTermObjects(int termId)
    {
        var term = new Term()
        {
            Id = termId,
        };
        var termDto = new TermDTO()
        {
            Id = term.Id,
        };

        return (term, termDto);
    }

    private static GetTermByIdQuery GetRequest(int termId)
    {
        return new GetTermByIdQuery(termId);
    }

    private void SetupMockRepository(GetTermByIdQuery request, Term? term)
    {
        _mockRepository
            .Setup(x => x.TermRepository.GetFirstOrDefaultAsync(
                t => t.Id == request.Id,
                It.IsAny<Func<IQueryable<Term>, IIncludableQueryable<Term, object>>>()))
            .ReturnsAsync(term);
    }
}
