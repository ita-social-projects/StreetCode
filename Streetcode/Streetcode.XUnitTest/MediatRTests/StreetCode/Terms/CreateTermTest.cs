using AutoMapper;
using FluentAssertions;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Term.Create;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms;

public class CreateTermTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotCreateLocalizer _mockCannotCreateLocalizer;
    private readonly MockFailedToCreateLocalizer _mockFailedToCreateLocalizer;
    private readonly MockCannotConvertNullLocalizer _mockCannotConvertNullLocalizer;
    private readonly CreateTermHandler _handler;

    public CreateTermTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCannotCreateLocalizer = new MockCannotCreateLocalizer();
        _mockFailedToCreateLocalizer = new MockFailedToCreateLocalizer();
        _mockCannotConvertNullLocalizer = new MockCannotConvertNullLocalizer();
        _handler = new CreateTermHandler(
            _mockMapper.Object,
            _mockRepository.Object,
            _mockLogger.Object,
            _mockCannotCreateLocalizer,
            _mockFailedToCreateLocalizer,
            _mockCannotConvertNullLocalizer);
    }

    [Fact]
    public async Task ShouldCreateSuccessfully_WhenRelatedTermAdded()
    {
        // Arrange
        var (termCreateDto, term, termDto) = GetTermObjects();
        var request = GetRequest(termCreateDto);

        SetupMockRepository(term, 1);
        MockHelpers.SetupMockMapper(_mockMapper, term, request.Term);
        MockHelpers.SetupMockMapper(_mockMapper, termDto, term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be(termCreateDto.Title);
        _mockRepository.Verify(x => x.TermRepository.CreateAsync(term), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        _mockMapper.Verify(x => x.Map<TermDTO>(term), Times.Once);
    }

    [Fact]
    public async Task ShouldCreateSuccessfully_WithCorrectDataType()
    {
        // Arrange
        var (termCreateDto, term, termDto) = GetTermObjects();
        var request = GetRequest(termCreateDto);

        SetupMockRepository(term, 1);
        MockHelpers.SetupMockMapper(_mockMapper, term, request.Term);
        MockHelpers.SetupMockMapper(_mockMapper, termDto, term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<TermDTO>();
    }

    [Fact]
    public async Task ShouldCreateSuccessfully_WhenSaveChangesAsyncFailed()
    {
        // Arrange
        var (termCreateDto, term, _) = GetTermObjects();
        var request = GetRequest(termCreateDto);
        var expectedErrorMessage = _mockFailedToCreateLocalizer["FailedToCreateTerm"].Value;

        SetupMockRepository(term, -1);
        MockHelpers.SetupMockMapper(_mockMapper, term, request.Term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Fact]
    public async Task ShouldCreateFailingly_WhenFirstMappingFailed()
    {
        // Arrange
        var (termCreateDto, _, _) = GetTermObjects();
        var request = GetRequest(termCreateDto);
        var expectedErrorMessage = _mockCannotConvertNullLocalizer["CannotConvertNullToTerm"].Value;

        MockHelpers.SetupMockMapper<Term?, TermCreateDTO>(_mockMapper, null, request.Term);

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
        var (termCreateDto, term, _) = GetTermObjects();
        var request = GetRequest(termCreateDto);
        var expectedErrorMessage = _mockFailedToCreateLocalizer["FailedToMapCreatedTerm"].Value;

        SetupMockRepository(term, 1);
        MockHelpers.SetupMockMapper(_mockMapper, term, request.Term);
        MockHelpers.SetupMockMapper<Term?, Term>(_mockMapper, null, term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (TermCreateDTO, Term, TermDTO) GetTermObjects()
    {
        const string title = "qwerty";

        var termCreateDto = new TermCreateDTO()
        {
            Title = title,
        };
        var term = new Term()
        {
            Title = title,
        };
        var termDto = new TermDTO()
        {
            Title = title,
        };

        return (termCreateDto, term, termDto);
    }

    private static CreateTermCommand GetRequest(TermCreateDTO termCreateDto)
    {
        return new CreateTermCommand(termCreateDto);
    }

    private void SetupMockRepository(Term term, int saveChangesResult)
    {
        _mockRepository
            .Setup(x => x.TermRepository.CreateAsync(term))
            .ReturnsAsync(term);
        _mockRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}
