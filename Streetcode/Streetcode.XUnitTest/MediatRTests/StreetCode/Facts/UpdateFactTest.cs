using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Update;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class UpdateFactTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockFailedToUpdateLocalizer _mockFailedToUpdateLocalizer;
    private readonly MockCannotConvertNullLocalizer _mockCannotConvertNullLocalizer;
    private readonly UpdateFactHandler _handler;

    public UpdateFactTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockFailedToUpdateLocalizer = new MockFailedToUpdateLocalizer();
        _mockCannotConvertNullLocalizer = new MockCannotConvertNullLocalizer();
        _handler = new UpdateFactHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockFailedToUpdateLocalizer,
            _mockCannotConvertNullLocalizer);
    }

    [Fact]
    public async Task ShouldUpdateSuccessfully_WhenFactExists()
    {
        // Arrange
        var (fact, factDto) = GetFactObjects();
        var request = GetRequest(factDto);

        SetupMockUpdateAndSaveChangesAsync(fact, 1);
        MockHelpers.SetupMockMapper(_mockMapper, fact, request.Fact);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(x => x.FactRepository.Update(fact), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldUpdateSuccessfully_WithCorrectDataType()
    {
        // Arrange
        var (fact, factDto) = GetFactObjects();
        var request = GetRequest(factDto);

        SetupMockUpdateAndSaveChangesAsync(fact, 1);
        MockHelpers.SetupMockMapper(_mockMapper, fact, request.Fact);

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
        var (_, factDto) = GetFactObjects();
        var request = GetRequest(factDto);
        var expectedErrorMessage = _mockCannotConvertNullLocalizer["CannotConvertNullToFact"].Value;

        MockHelpers.SetupMockMapper<Fact?, StreetcodeFactUpdateDTO>(_mockMapper, null, request.Fact);

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
        var (fact, factDto) = GetFactObjects();
        var request = GetRequest(factDto);
        var expectedErrorMessage = _mockFailedToUpdateLocalizer["FailedToUpdateFact"].Value;

        SetupMockUpdateAndSaveChangesAsync(fact, -1);
        MockHelpers.SetupMockMapper(_mockMapper, fact, request.Fact);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (Fact, StreetcodeFactUpdateDTO) GetFactObjects()
    {
        var fact = new Fact();
        var factDto = new StreetcodeFactUpdateDTO();

        return (fact, factDto);
    }

    private static UpdateFactCommand GetRequest(StreetcodeFactUpdateDTO factDto)
    {
        return new UpdateFactCommand(factDto);
    }

    private void SetupMockUpdateAndSaveChangesAsync(Fact fact, int saveChangesResult)
    {
        _mockRepository
            .Setup(x => x.FactRepository.Update(fact));
        _mockRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}
