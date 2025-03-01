using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Term.Update;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms;

public class UpdateTermTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotConvertNullLocalizer _mockCannotConvertNullLocalizer;
    private readonly MockFailedToUpdateLocalizer _mockFailedToUpdateLocalizer;
    private readonly UpdateTermHandler _handler;

    public UpdateTermTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCannotConvertNullLocalizer = new MockCannotConvertNullLocalizer();
        _mockFailedToUpdateLocalizer = new MockFailedToUpdateLocalizer();
        _handler = new UpdateTermHandler(
            _mockMapper.Object,
            _mockRepository.Object,
            _mockLogger.Object,
            _mockFailedToUpdateLocalizer,
            _mockCannotConvertNullLocalizer);
    }

    [Fact]
    public async Task ShouldUpdateSuccessfully_WhenTermExists()
    {
        // Arrange
        var (term, termDto) = GetTermObjects();
        var request = GetRequest(termDto);

        SetupMockUpdateAndSaveChangesAsync(term, 1);
        MockHelpers.SetupMockMapper(_mockMapper, term, request.Term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(x => x.TermRepository.Update(term), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldUpdateSuccessfully_WithCorrectDataType()
    {
        // Arrange
        var (term, termDto) = GetTermObjects();
        var request = GetRequest(termDto);

        SetupMockUpdateAndSaveChangesAsync(term, 1);
        MockHelpers.SetupMockMapper(_mockMapper, term, request.Term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<Unit>();
    }

    [Fact]
    public async Task ShouldUpdateFailingly_WhenMappingFailed()
    {
        // Arrange
        var (_, termDto) = GetTermObjects();
        var request = GetRequest(termDto);
        var expectedErrorMessage = _mockCannotConvertNullLocalizer["CannotConvertNullToTerm"].Value;

        MockHelpers.SetupMockMapper<Term?, TermDTO>(_mockMapper, null, request.Term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Fact]
    public async Task ShouldUpdateFailingly_WhenSaveChangesAsyncFailed()
    {
        // Arrange
        var (term, termDto) = GetTermObjects();
        var request = GetRequest(termDto);
        var expectedErrorMessage = _mockFailedToUpdateLocalizer["FailedToUpdateTerm"].Value;

        SetupMockUpdateAndSaveChangesAsync(term, -1);
        MockHelpers.SetupMockMapper(_mockMapper, term, request.Term);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (Term, TermDTO) GetTermObjects()
    {
        var term = new Term();
        var termDto = new TermDTO();

        return (term, termDto);
    }

    private static UpdateTermCommand GetRequest(TermDTO termDto)
    {
        return new UpdateTermCommand(termDto);
    }

    private void SetupMockUpdateAndSaveChangesAsync(Term term, int saveChangesResult)
    {
        _mockRepository
            .Setup(x => x.TermRepository.Update(term));
        _mockRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}
