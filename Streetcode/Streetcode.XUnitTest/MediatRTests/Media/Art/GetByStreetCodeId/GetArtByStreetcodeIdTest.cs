using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Arts;

public class GetArtByStreetcodeIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IBlobService> _blobService;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;
    private readonly GetArtsByStreetcodeIdHandler _handler;

    public GetArtByStreetcodeIdTest()
    {
        _mockRepo = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _blobService = new Mock<IBlobService>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        _handler = new GetArtsByStreetcodeIdHandler(
            _mockRepo.Object,
            _mockMapper.Object,
            _blobService.Object,
            _mockLogger.Object);
    }

    [Theory]
    [InlineData(1)]

    public async Task Handle_ReturnsArt(int streetcodeId)
    {
        // Arrange
        MockRepositoryAndMapper(GetArtsList(), GetArtsDtoList());

        // Act
        var result = await _handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        // Assert
        Assert.Equal(streetcodeId, result.Value.First().Id);
    }

    [Theory]
    [InlineData(1)]

    public async Task Handle_ReturnsEmptyArray(int streetcodeId)
    {
        // Arrange
        MockRepositoryAndMapper(new List<Art>(), new List<ArtDTO>());

        // Act
        var result = await _handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<Result<IEnumerable<ArtDTO>>>(result),
            () => Assert.IsAssignableFrom<IEnumerable<ArtDTO>>(result.Value),
            () => Assert.Empty(result.Value));
    }

    [Theory]
    [InlineData(1)]

    public async Task Handle_ReturnsType(int streetcodeId)
    {
        // Arrange
        MockRepositoryAndMapper(GetArtsList(), GetArtsDtoList());

        // Act
        var result = await _handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        // Assert
        Assert.IsType<Result<IEnumerable<ArtDTO>>>(result);
    }

    private static List<Art> GetArtsList()
    {
        return new List<Art>()
        {
            new Art()
            {
                Id = 1,
                Image = new DAL.Entities.Media.Images.Image(),
            },
            new Art()
            {
                Id = 2,
                Image = new DAL.Entities.Media.Images.Image(),
            },
        };
    }

    private static List<ArtDTO> GetArtsDtoList()
    {
        return new List<ArtDTO>()
        {
            new ArtDTO
            {
                Id = 1,
                Image = new ImageDTO(),
            },
            new ArtDTO
            {
                Id = 2,
                Image = new ImageDTO(),
            },
        };
    }

    private void MockRepositoryAndMapper(List<Art> artList, List<ArtDTO> artListDto)
    {
        _mockRepo
            .Setup(r => r.ArtRepository
                .GetAllAsync(
                    It.IsAny<Expression<Func<Art, bool>>>(),
                    It.IsAny<Func<IQueryable<Art>,
                        IIncludableQueryable<Art, object>>>()))
            .ReturnsAsync(artList);

        _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(artListDto);

        _mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
            .Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int streetcodeId)
                {
                    return new LocalizedString(key, $"Cannot find any art with corresponding streetcode id: {streetcodeId}");
                }

                return new LocalizedString(key, "Cannot find any art with corresponding streetcode id");
            });
    }
}