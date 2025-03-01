using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetAllByTermId;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using RelatedTermEntity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.RelatedTerms;

public class GetAllRelatedTermsByTermIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotCreateLocalizer _mockCannotCreateLocalizer;
    private readonly GetAllRelatedTermsByTermIdHandler _handler;

    public GetAllRelatedTermsByTermIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCannotCreateLocalizer = new MockCannotCreateLocalizer();
        _handler = new GetAllRelatedTermsByTermIdHandler(
            _mockMapper.Object,
            _mockRepository.Object,
            _mockLogger.Object,
            _mockCannotCreateLocalizer);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetAllByTermIdSuccessfully_WhenRelatedTermsExist(int termId)
    {
        // Arrange
        var (relatedTermsList, relatedTermDtoList) = GetRelatedTermsObjectsLists(termId);
        var request = GetRequest(termId);

        MockHelpers.SetupMockRelatedTermRepositoryGetAllAsync(_mockRepository, relatedTermsList);
        MockHelpers.SetupMockMapper<IEnumerable<RelatedTermDTO>, List<RelatedTermEntity>>(
            _mockMapper,
            relatedTermDtoList,
            relatedTermsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().SatisfyRespectively(
            first =>
            {
                first.Id.Should().Be(relatedTermsList[0].Id);
                first.TermId.Should().Be(relatedTermsList[0].TermId);
            },
            second =>
            {
                second.Id.Should().Be(relatedTermsList[1].Id);
                second.TermId.Should().Be(relatedTermsList[1].TermId);
            });
        result.Value.Should().HaveCount(2);
        VerifyGetAllAsyncAndMockingOperationsExecution(request, relatedTermsList);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetAllByTermIdSuccessfully_WithCorrectDataType(int termId)
    {
        // Arrange
        var (relatedTermsList, relatedTermDtoList) = GetRelatedTermsObjectsLists(termId);
        var request = GetRequest(termId);

        MockHelpers.SetupMockRelatedTermRepositoryGetAllAsync(_mockRepository, relatedTermsList);
        MockHelpers.SetupMockMapper<IEnumerable<RelatedTermDTO>, List<RelatedTermEntity>>(
            _mockMapper,
            relatedTermDtoList,
            relatedTermsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<List<RelatedTermDTO>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetAllByTermIdSuccessfully_WhenRelatedTermsNotExist(int termId)
    {
        // Arrange
        var (emptyRelatedTermsList, emptyRelatedTermDtoList) = GetEmptyRelatedTermsObjectsLists();
        var request = GetRequest(termId);

        MockHelpers.SetupMockRelatedTermRepositoryGetAllAsync(_mockRepository, emptyRelatedTermsList);
        MockHelpers.SetupMockMapper(_mockMapper, emptyRelatedTermDtoList, emptyRelatedTermsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
        result.ValueOrDefault.Should().BeAssignableTo<IEnumerable<RelatedTermDTO>>();
        VerifyGetAllAsyncAndMockingOperationsExecution(request, emptyRelatedTermsList);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteFailingly_WhenMappingFailed(int termId)
    {
        // Arrange
        var (relatedTermsList, _) = GetRelatedTermsObjectsLists(termId);
        var request = GetRequest(termId);
        var expectedErrorMessage = _mockCannotCreateLocalizer["CannotCreateDTOsForRelatedWords"].Value;

        MockHelpers.SetupMockRelatedTermRepositoryGetAllAsync(_mockRepository, relatedTermsList);
        MockHelpers.SetupMockMapper<IEnumerable<RelatedTermDTO>?, List<RelatedTermEntity>>(
            _mockMapper,
            null,
            relatedTermsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (List<RelatedTermEntity>, List<RelatedTermDTO>) GetRelatedTermsObjectsLists(int termId)
    {
        var relatedTermsList = new List<RelatedTermEntity>()
        {
            new RelatedTermEntity()
            {
                Id = 1,
                TermId = termId,
            },
            new RelatedTermEntity()
            {
                Id = 2,
                TermId = termId,
            },
        };
        var relatedTermDtoList = new List<RelatedTermDTO>()
        {
            new RelatedTermDTO()
            {
                Id = relatedTermsList[0].Id,
                TermId = termId,
            },
            new RelatedTermDTO()
            {
                Id = relatedTermsList[1].Id,
                TermId = termId,
            },
        };

        return (relatedTermsList, relatedTermDtoList);
    }

    private static (List<RelatedTermEntity>, List<RelatedTermDTO>) GetEmptyRelatedTermsObjectsLists()
    {
        return (new List<RelatedTermEntity>(), new List<RelatedTermDTO>());
    }

    private static GetAllRelatedTermsByTermIdQuery GetRequest(int termId)
    {
        return new GetAllRelatedTermsByTermIdQuery(termId);
    }

    private void VerifyGetAllAsyncAndMockingOperationsExecution(
        GetAllRelatedTermsByTermIdQuery request,
        List<RelatedTermEntity> relatedTermsList)
    {
        _mockRepository.Verify(
            x => x.RelatedTermRepository.GetAllAsync(
                rt => rt.TermId == request.Id,
                It.IsAny<Func<IQueryable<RelatedTermEntity>, IIncludableQueryable<RelatedTermEntity, object>>?>()),
            Times.Once);
        _mockMapper.Verify(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTermsList), Times.Once);
    }
}
