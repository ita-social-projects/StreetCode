using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Create;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class CreateFactTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockFailedToCreateLocalizer _mockFailedToCreateLocalizer;
    private readonly MockCannotConvertNullLocalizer mockCannotConvertNullLocalizer;
    private readonly CreateFactHandler _handler;

    public CreateFactTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockFailedToCreateLocalizer = new MockFailedToCreateLocalizer();
        mockCannotConvertNullLocalizer = new MockCannotConvertNullLocalizer();
        _handler = new CreateFactHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockFailedToCreateLocalizer,
            mockCannotConvertNullLocalizer);
    }

    [Fact]
    public async Task ShouldCreateSuccessfully_WhenFactAdded()
    {
        // Arrange
        var (factDto, fact) = GetFactObjects();
        var request = GetRequest(factDto);

        SetupMockRepository(fact, 1);
        MockHelpers.SetupMockMapper(_mockMapper, fact, request.Fact);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(x => x.FactRepository.CreateAsync(fact), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldCreateSuccessfully_WithCorrectDataType()
    {
        // Arrange
        var (factDto, fact) = GetFactObjects();
        var request = GetRequest(factDto);

        SetupMockRepository(fact, 1);
        MockHelpers.SetupMockMapper(_mockMapper, fact, request.Fact);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<Unit>();
    }

    [Fact]
    public async Task ShouldCreateFailingly_WhenMappingFailed()
    {
        // Arrange
        var (factDto, _) = GetFactObjects();
        var request = GetRequest(factDto);
        var expectedErrorMessage = mockCannotConvertNullLocalizer["CannotConvertNullToFact"].Value;

        MockHelpers.SetupMockMapper<Fact?, StreetcodeFactCreateDTO>(_mockMapper, null, request.Fact);

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
        var (factDto, fact) = GetFactObjects();
        var request = GetRequest(factDto);
        var expectedErrorMessage = _mockFailedToCreateLocalizer["FailedToCreateFact"].Value;

        SetupMockRepository(fact, -1);
        MockHelpers.SetupMockMapper(_mockMapper, fact, request.Fact);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (StreetcodeFactCreateDTO, Fact) GetFactObjects()
    {
        var factCreateDto = new StreetcodeFactCreateDTO();
        var fact = new Fact();

        return (factCreateDto, fact);
    }

    private static CreateFactCommand GetRequest(StreetcodeFactCreateDTO factDto)
    {
        return new CreateFactCommand(factDto);
    }

    private void SetupMockRepository(Fact fact, int saveChangesResult)
    {
        _mockRepository
            .Setup(x => x.FactRepository.CreateAsync(fact));
        _mockRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}
