using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.MediatR.Streetcode.Text.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Texts;

public class GetAllTextsTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllTextsHandler _handler;

    public GetAllTextsTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllTextsHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WhenTermsExist()
    {
        // Arrange
        var (textsList, textDtoList) = GetTextObjectsLists();
        var request = GetRequest();

        MockHelpers.SetupMockTextRepositoryGetAllAsync(_mockRepository, textsList);
        MockHelpers.SetupMockMapper<IEnumerable<TextDTO>, List<Text>>(_mockMapper, textDtoList, textsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().SatisfyRespectively(
            first => first.Id.Should().Be(textsList[0].Id),
            second => second.Id.Should().Be(textsList[1].Id));
        result.Value.Should().HaveCount(2);
        VerifyGetAllAsyncAndMockingOperationsExecution(textsList);
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WithCorrectDataType()
    {
        // Arrange
        var (textsList, textDtoList) = GetTextObjectsLists();
        var request = GetRequest();

        MockHelpers.SetupMockTextRepositoryGetAllAsync(_mockRepository, textsList);
        MockHelpers.SetupMockMapper<IEnumerable<TextDTO>, List<Text>>(_mockMapper, textDtoList, textsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<List<TextDTO>>();
    }

    [Fact]
    public async Task ShouldGetAllSuccessfully_WhenTermsNotExist()
    {
        // Arrange
        var (textsList, textDtoList) = GetEmptyTextObjectsLists();
        var request = GetRequest();

        MockHelpers.SetupMockTextRepositoryGetAllAsync(_mockRepository, textsList);
        MockHelpers.SetupMockMapper(_mockMapper, textDtoList, textsList);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
        result.ValueOrDefault.Should().BeAssignableTo<IEnumerable<TextDTO>>();
        VerifyGetAllAsyncAndMockingOperationsExecution(textsList);
    }

    private static (List<Text>, List<TextDTO>) GetTextObjectsLists()
    {
        var textsList = new List<Text>()
        {
            new Text()
            {
                Id = 1,
            },
            new Text()
            {
                Id = 2,
            },
        };
        var textDtoList = new List<TextDTO>()
        {
            new TextDTO()
            {
                Id = textsList[0].Id,
            },
            new TextDTO()
            {
                Id = textsList[1].Id,
            },
        };

        return (textsList, textDtoList);
    }

    private static (List<Text>, List<TextDTO>) GetEmptyTextObjectsLists()
    {
        return (new List<Text>(), new List<TextDTO>());
    }

    private static GetAllTextsQuery GetRequest()
    {
        return new GetAllTextsQuery();
    }

    private void VerifyGetAllAsyncAndMockingOperationsExecution(List<Text> textsList)
    {
        _mockRepository.Verify(
            x => x.TextRepository.GetAllAsync(
                It.IsAny<Expression<Func<Text, bool>>>(),
                It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, object>>>()),
            Times.Once);
        _mockMapper.Verify(x => x.Map<IEnumerable<TextDTO>>(textsList), Times.Once);
    }
}
