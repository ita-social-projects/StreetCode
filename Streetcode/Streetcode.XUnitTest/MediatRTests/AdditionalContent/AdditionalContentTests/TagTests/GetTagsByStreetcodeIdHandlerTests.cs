using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests;

public class GetTagsByStreetcodeIdHandlerTests
{
    private const int StreetcodeId = 1;
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

    private readonly List<StreetcodeTagIndex> _tags = new List<StreetcodeTagIndex>()
    {
        new StreetcodeTagIndex
        {
            Index = 1,
            IsVisible = true,
            Streetcode = new StreetcodeContent
            {
                Id = StreetcodeId,
            },
            StreetcodeId = StreetcodeId,
            Tag = new Tag()
            {
                Id = 1,
                Title = "title",
            },
        },
        new StreetcodeTagIndex
        {
            Index = 2,
            IsVisible = true,
            Streetcode = new StreetcodeContent
            {
                Id = StreetcodeId,
            },
            StreetcodeId = StreetcodeId,
            Tag = new Tag()
            {
                Id = 2,
                Title = "title",
            },
        },
    };

    private readonly List<StreetcodeTagDTO> _tagDtOs = new List<StreetcodeTagDTO>()
    {
        new StreetcodeTagDTO
        {
            Id = 1,
            Title = "title",
            IsVisible = true,
            Index = 1,
        },
        new StreetcodeTagDTO
        {
            Id = 2,
            Title = "title",
            IsVisible = true,
            Index = 2,
        },
    };

    public GetTagsByStreetcodeIdHandlerTests()
    {
        _mockRepo = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task Handler_Returns_NotEmpty_List()
    {
        // Arrange
        SetupRepository(_tags);
        SetupMapper(_tagDtOs);

        var handler = new GetTagByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetTagByStreetcodeIdQuery(StreetcodeId), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<List<StreetcodeTagDTO>>(result.Value),
            () => Assert.NotEmpty(result.Value));
    }

    [Fact]
    public async Task Handler_Returns_Empty_List()
    {
        // Arrange
        SetupRepository(new List<StreetcodeTagIndex>());

        var handler = new GetTagByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetTagByStreetcodeIdQuery(StreetcodeId), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<Result<IEnumerable<StreetcodeTagDTO>>>(result),
            () => Assert.IsAssignableFrom<IEnumerable<StreetcodeTagDTO>>(result.Value),
            () => Assert.Empty(result.Value));
    }

    private void SetupRepository(List<StreetcodeTagIndex> returnList)
    {
        _mockRepo.Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
                    IIncludableQueryable<StreetcodeTagIndex, object>>>()))
            .ReturnsAsync(returnList);
    }

    private void SetupMapper(List<StreetcodeTagDTO> returnList)
    {
        _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeTagDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(returnList);
    }
}