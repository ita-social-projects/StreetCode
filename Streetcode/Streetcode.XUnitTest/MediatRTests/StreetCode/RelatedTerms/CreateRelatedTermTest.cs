using AutoMapper;
using FluentAssertions;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using RelatedTermEntity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.RelatedTerms;

public class CreateRelatedTermTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotSaveLocalizer _mockCannotSaveLocalizer;
    private readonly MockCannotMapLocalizer _mockCannotMapLocalizer;
    private readonly MockCreateRelatedTermLocalizer _mockCreateRelatedTermLocalizer;
    private readonly MockCannotCreateLocalizer _mockCannotCreateLocalizer;
    private readonly CreateRelatedTermHandler _handler;

    public CreateRelatedTermTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCannotSaveLocalizer = new MockCannotSaveLocalizer();
        _mockCannotMapLocalizer = new MockCannotMapLocalizer();
        _mockCreateRelatedTermLocalizer = new MockCreateRelatedTermLocalizer();
        _mockCannotCreateLocalizer = new MockCannotCreateLocalizer();
        _handler = new CreateRelatedTermHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockCannotSaveLocalizer,
            _mockCannotMapLocalizer,
            _mockCreateRelatedTermLocalizer,
            _mockCannotCreateLocalizer);
    }

    [Fact]
    public async Task ShouldCreateSuccessfully_WhenRelatedTermAdded()
    {
        // Arrange
        var (relatedTermCreateDto, relatedTerm, relatedTermDto) = GetRelatedTermObjects();
        var existingTermsList = GetEmptyRelatedTermList();
        var request = GetRequest(relatedTermCreateDto);

        MockHelpers.SetupMockRelatedTermRepositoryGetAllAsync(_mockRepository, existingTermsList);
        SetupMockCreateAndSaveChangesAsync(relatedTerm, 1);
        MockHelpers.SetupMockMapper(_mockMapper, relatedTerm, request.RelatedTerm);
        MockHelpers.SetupMockMapper(_mockMapper, relatedTermDto, relatedTerm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Word.Should().Be(relatedTerm.Word);
        _mockRepository.Verify(r => r.RelatedTermRepository.CreateAsync(relatedTerm), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mockMapper.Verify(x => x.Map<RelatedTermDTO>(relatedTerm), Times.Once);
    }

    [Fact]
    public async Task ShouldCreateSuccessfully_WithCorrectDataType()
    {
        // Arrange
        var (relatedTermCreateDto, relatedTerm, relatedTermDto) = GetRelatedTermObjects();
        var existingTermsList = GetEmptyRelatedTermList();
        var request = GetRequest(relatedTermCreateDto);

        MockHelpers.SetupMockRelatedTermRepositoryGetAllAsync(_mockRepository, existingTermsList);
        SetupMockCreateAndSaveChangesAsync(relatedTerm, 1);
        MockHelpers.SetupMockMapper(_mockMapper, relatedTerm, request.RelatedTerm);
        MockHelpers.SetupMockMapper(_mockMapper, relatedTermDto, relatedTerm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<RelatedTermDTO>();
    }

    [Fact]
    public async Task ShouldCreateFailingly_WhenFirstMappingFailed()
    {
        // Arrange
        var (relatedTermCreateDto, _, _) = GetRelatedTermObjects();
        var request = GetRequest(relatedTermCreateDto);
        var expectedErrorMessage = _mockCannotCreateLocalizer["CannotCreateNewRelatedWordForTerm"].Value;

        MockHelpers.SetupMockMapper<RelatedTermEntity?, RelatedTermCreateDTO>(_mockMapper, null, request.RelatedTerm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Fact]
    public async Task ShouldCreateFailingly_WhenRelatedTermAlreadyExists()
    {
        // Arrange
        var (relatedTermCreateDto, relatedTerm, _) = GetRelatedTermObjects();
        var existingTermsList = GetRelatedTermList();
        var request = GetRequest(relatedTermCreateDto);
        var expectedErrorMessage = _mockCreateRelatedTermLocalizer["WordWithThisDefinitionAlreadyExists"].Value;

        MockHelpers.SetupMockRelatedTermRepositoryGetAllAsync(_mockRepository, existingTermsList);
        MockHelpers.SetupMockMapper(_mockMapper, relatedTerm, request.RelatedTerm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Fact]
    public async Task ShouldCreateFailingly_WhenSaveChangesAsyncFailed()
    {
        // Arrange
        var (relatedTermCreateDto, relatedTerm, _) = GetRelatedTermObjects();
        var existingTermsList = GetEmptyRelatedTermList();
        var request = GetRequest(relatedTermCreateDto);
        var expectedErrorMessage = _mockCannotSaveLocalizer["CannotSaveChangesInTheDatabaseAfterRelatedWordCreation"].Value;

        MockHelpers.SetupMockRelatedTermRepositoryGetAllAsync(_mockRepository, existingTermsList);
        SetupMockCreateAndSaveChangesAsync(relatedTerm, -1);
        MockHelpers.SetupMockMapper(_mockMapper, relatedTerm, request.RelatedTerm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Fact]
    public async Task ShouldCreateFailingly_WhenSecondMappingFailed()
    {
        // Arrange
        var (relatedTermCreateDto, relatedTerm, _) = GetRelatedTermObjects();
        var existingTermsList = GetEmptyRelatedTermList();
        var request = GetRequest(relatedTermCreateDto);
        var expectedErrorMessage = _mockCannotMapLocalizer["CannotMapEntity"].Value;

        MockHelpers.SetupMockRelatedTermRepositoryGetAllAsync(_mockRepository, existingTermsList);
        SetupMockCreateAndSaveChangesAsync(relatedTerm, 1);
        MockHelpers.SetupMockMapper(_mockMapper, relatedTerm, request.RelatedTerm);
        MockHelpers.SetupMockMapper<RelatedTermDTO?, RelatedTermEntity>(_mockMapper, null, relatedTerm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (RelatedTermCreateDTO, RelatedTermEntity, RelatedTermDTO) GetRelatedTermObjects()
    {
        const string word = "qwerty";

        var relatedTermCreateDto = new RelatedTermCreateDTO()
        {
            Word = word,
        };
        var relatedTerm = new RelatedTermEntity()
        {
            Word = word,
        };
        var relatedTermDto = new RelatedTermDTO()
        {
            Word = word,
        };

        return (relatedTermCreateDto, relatedTerm, relatedTermDto);
    }

    private static List<RelatedTermEntity> GetRelatedTermList()
    {
        return new List<RelatedTermEntity>()
        {
            new RelatedTermEntity(),
            new RelatedTermEntity(),
        };
    }

    private static List<RelatedTermEntity> GetEmptyRelatedTermList()
    {
        return new List<RelatedTermEntity>();
    }

    private static CreateRelatedTermCommand GetRequest(RelatedTermCreateDTO relatedTermDto)
    {
        return new CreateRelatedTermCommand(relatedTermDto);
    }

    private void SetupMockCreateAndSaveChangesAsync(RelatedTermEntity relatedTerm, int saveChangesResult)
    {
        _mockRepository
            .Setup(x => x.RelatedTermRepository.CreateAsync(relatedTerm))
            .ReturnsAsync(relatedTerm);
        _mockRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}
