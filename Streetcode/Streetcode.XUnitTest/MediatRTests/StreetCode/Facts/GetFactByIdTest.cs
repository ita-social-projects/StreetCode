using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetById;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class GetFactByIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly GetFactByIdHandler _handler;

    public GetFactByIdTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _handler = new GetFactByIdHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockCannotFindLocalizer);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByIdSuccessfully_WhenIdExists(int factId)
    {
        // Arrange
        var (fact, factDto) = GetFactObjects(factId);
        var request = GetRequest(factId);

        MockHelpers.SetupMockFactRepositoryGetFirstOrDefaultAsync(_mockRepository, fact);
        MockHelpers.SetupMockMapper(_mockMapper, factDto, fact);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(factId);
        _mockMapper.Verify(x => x.Map<FactDto>(fact), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByIdSuccessfully_WithCorrectDataType(int factId)
    {
        // Arrange
        var (fact, factDto) = GetFactObjects(factId);
        var request = GetRequest(factId);

        MockHelpers.SetupMockFactRepositoryGetFirstOrDefaultAsync(_mockRepository, fact);
        MockHelpers.SetupMockMapper(_mockMapper, factDto, fact);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<FactDto>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByIdFailingly_WhenFactNotFound(int factId)
    {
        // Arrange
        var request = GetRequest(factId);
        var expectedErrorMessage = _mockCannotFindLocalizer["CannotFindFactWithCorrespondingCategoryId", factId].Value;

        MockHelpers.SetupMockFactRepositoryGetFirstOrDefaultAsync(_mockRepository, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (Fact, FactDto) GetFactObjects(int factId)
    {
        var fact = new Fact()
        {
            Id = factId,
        };
        var factDto = new FactDto()
        {
            Id = fact.Id,
        };

        return (fact, factDto);
    }

    private static GetFactByIdQuery GetRequest(int factId)
    {
        return new GetFactByIdQuery(factId, UserRole.User);
    }
}
