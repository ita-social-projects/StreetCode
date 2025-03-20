using AutoMapper;
using FluentAssertions;
using FluentResults;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Texts;

public class GetTextByStreetcodeIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly GetTextByStreetcodeIdHandler _handler;

    public GetTextByStreetcodeIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCacheService = new Mock<ICacheService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _handler = new GetTextByStreetcodeIdHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockCannotFindLocalizer,
            _mockCacheService.Object);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByStreetcodeIdSuccessfully_WhenIdExists(int streetcodeId)
    {
        // Arrange
        var resultTextDto = GetResultTextDto(streetcodeId);
        var request = GetRequest(streetcodeId);
        var cacheKey = $"TextCache_{request.StreetcodeId}";

        SetupMockCacheService(cacheKey, resultTextDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(resultTextDto.Value.Id);
        result.Value.StreetcodeId.Should().Be(request.StreetcodeId);
        VerifyGetOrSetAsyncOperationExecution(cacheKey);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByStreetcodeIdSuccessfully_WithCorrectDataType(int streetcodeId)
    {
        // Arrange
        var resultTextDto = GetResultTextDto(streetcodeId);
        var request = GetRequest(streetcodeId);
        var cacheKey = $"TextCache_{request.StreetcodeId}";

        SetupMockCacheService(cacheKey, resultTextDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<TextDTO>();
    }

    private static Result<TextDTO> GetResultTextDto(int streetcodeId)
    {
        var textDto = new TextDTO()
        {
            Id = 1,
            StreetcodeId = streetcodeId,
        };

        return Result.Ok(textDto);
    }

    private static GetTextByStreetcodeIdQuery GetRequest(int streetcodeId)
    {
        return new GetTextByStreetcodeIdQuery(streetcodeId, UserRole.User);
    }

    private void SetupMockCacheService(string cacheKey, Result<TextDTO> resultTextDto)
    {
        _mockCacheService
            .Setup(x => x.GetOrSetAsync(
                cacheKey,
                It.IsAny<Func<Task<Result<TextDTO>>>>(),
                TimeSpan.FromMinutes(10)))
            .ReturnsAsync(resultTextDto);
    }

    private void VerifyGetOrSetAsyncOperationExecution(string cacheKey)
    {
        _mockCacheService.Verify(
            x => x.GetOrSetAsync(
                cacheKey,
                It.IsAny<Func<Task<Result<TextDTO>>>>(),
                TimeSpan.FromMinutes(10)),
            Times.Once);
    }
}
