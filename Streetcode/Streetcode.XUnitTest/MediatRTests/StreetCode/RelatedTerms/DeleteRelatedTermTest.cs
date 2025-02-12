using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using RelatedTermEntity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.RelatedTerms;

public class DeleteRelatedTermTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockFailedToDeleteLocalizer _mockFailedToDeleteLocalizer;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly DeleteRelatedTermHandler _handler;

    public DeleteRelatedTermTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockFailedToDeleteLocalizer = new MockFailedToDeleteLocalizer();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _handler = new DeleteRelatedTermHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockFailedToDeleteLocalizer,
            _mockCannotFindLocalizer);
    }

    [Theory]
    [InlineData("qwerty")]
    public async Task ShouldDeleteSuccessfully_WhenRelatedTermExists(string word)
    {
        // Arrange
        var (relatedTerm, relatedTermDto) = GetRelatedTermObjects(word);
        var request = GetRequest(word);

        SetupMockGetFirstOrDefaultAsync(request, relatedTerm);
        SetupMockDeleteAndSaveChangesAsync(relatedTerm, 1);
        MockHelpers.SetupMockMapper(_mockMapper, relatedTermDto, relatedTerm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Word.Should().Be(word);
        _mockRepository.Verify(x => x.RelatedTermRepository.Delete(relatedTerm), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData("qwerty")]
    public async Task ShouldDeleteSuccessfully_WithCorrectDataType(string word)
    {
        // Arrange
        var (relatedTerm, relatedTermDto) = GetRelatedTermObjects(word);
        var request = GetRequest(word);

        SetupMockGetFirstOrDefaultAsync(request, relatedTerm);
        SetupMockDeleteAndSaveChangesAsync(relatedTerm, 1);
        MockHelpers.SetupMockMapper(_mockMapper, relatedTermDto, relatedTerm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<RelatedTermDTO>();
    }

    [Theory]
    [InlineData("qwerty")]
    public async Task ShouldDeleteFailingly_WhenRelatedTermNotExist(string word)
    {
        // Arrange
        var request = GetRequest(word);
        var expectedErrorMessage = _mockCannotFindLocalizer["CannotFindRelatedTermWithCorrespondingId", request.word].Value;

        SetupMockGetFirstOrDefaultAsync(request, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Theory]
    [InlineData("qwerty")]
    public async Task ShouldDeleteFailingly_WhenSaveChangesAsyncFailed(string word)
    {
        // Arrange
        var (relatedTerm, _) = GetRelatedTermObjects(word);
        var request = GetRequest(word);
        var expectedErrorMessage = _mockFailedToDeleteLocalizer["FailedToDeleteRelatedTerm"].Value;

        SetupMockGetFirstOrDefaultAsync(request, relatedTerm);
        SetupMockDeleteAndSaveChangesAsync(relatedTerm, -1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Theory]
    [InlineData("qwerty")]
    public async Task ShouldDeleteFailingly_WhenMappingFailed(string word)
    {
        // Arrange
        var (relatedTerm, _) = GetRelatedTermObjects(word);
        var request = GetRequest(word);
        var expectedErrorMessage = _mockFailedToDeleteLocalizer["FailedToDeleteRelatedTerm"].Value;

        SetupMockGetFirstOrDefaultAsync(request, relatedTerm);
        SetupMockDeleteAndSaveChangesAsync(relatedTerm, 1);
        MockHelpers.SetupMockMapper<RelatedTermDTO?, RelatedTermEntity>(_mockMapper, null, relatedTerm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (RelatedTermEntity, RelatedTermDTO) GetRelatedTermObjects(string word)
    {
        var relatedTerm = new RelatedTermEntity()
        {
            Word = word,
        };
        var relatedTermDto = new RelatedTermDTO()
        {
            Word = word,
        };

        return (relatedTerm, relatedTermDto);
    }

    private static DeleteRelatedTermCommand GetRequest(string word)
    {
        return new DeleteRelatedTermCommand(word);
    }

    private void SetupMockGetFirstOrDefaultAsync(DeleteRelatedTermCommand request, RelatedTermEntity? getFirstOrDefaultAsyncResult)
    {
        _mockRepository
            .Setup(x => x.RelatedTermRepository.GetFirstOrDefaultAsync(
                x => x.Word!.ToLower().Equals(request.word.ToLower()),
                It.IsAny<Func<IQueryable<RelatedTermEntity>, IIncludableQueryable<RelatedTermEntity, object>>>()))
            .ReturnsAsync(getFirstOrDefaultAsyncResult);
    }

    private void SetupMockDeleteAndSaveChangesAsync(RelatedTermEntity relatedTerm, int saveChangesResult)
    {
        _mockRepository
            .Setup(x => x.RelatedTermRepository.Delete(relatedTerm));
        _mockRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}
