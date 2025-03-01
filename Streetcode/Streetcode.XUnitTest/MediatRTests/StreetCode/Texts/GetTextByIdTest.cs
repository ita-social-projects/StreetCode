using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Text.GetById;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Texts;

public class GetTextByIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly GetTextByIdHandler _handler;

    public GetTextByIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _handler = new GetTextByIdHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockCannotFindLocalizer);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByIdSuccessfully_WhenIdExists(int textId)
    {
        // Arrange
        var (text, textDto) = GetTextObjects(textId);
        var request = GetRequest(textId);

        MockHelpers.SetupMockTextGetFirstOrDefaultAsync(_mockRepository, text);
        MockHelpers.SetupMockMapper(_mockMapper, textDto, text);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(text.Id);
        _mockRepository.Verify(
            x => x.TextRepository.GetFirstOrDefaultAsync(
                t => t.Id == request.Id,
                It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, object>>>()),
            Times.Once);
        _mockMapper.Verify(x => x.Map<TextDTO>(text), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByIdSuccessfully_WithCorrectDataType(int textId)
    {
        // Arrange
        var (text, textDto) = GetTextObjects(textId);
        var request = GetRequest(textId);

        MockHelpers.SetupMockTextGetFirstOrDefaultAsync(_mockRepository, text);
        MockHelpers.SetupMockMapper(_mockMapper, textDto, text);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<TextDTO>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetByIdFailingly_WhenTextNotFound(int textId)
    {
        // Arrange
        var request = GetRequest(textId);
        var expectedErrorMessage = _mockCannotFindLocalizer["CannotFindAnyTextWithCorrespondingId", request.Id].Value;

        MockHelpers.SetupMockTextGetFirstOrDefaultAsync(_mockRepository, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static (Text, TextDTO) GetTextObjects(int textId)
    {
        var text = new Text()
        {
            Id = textId,
        };
        var textDto = new TextDTO()
        {
            Id = text.Id,
        };

        return (text, textDto);
    }

    private static GetTextByIdQuery GetRequest(int textId)
    {
        return new GetTextByIdQuery(textId);
    }
}
