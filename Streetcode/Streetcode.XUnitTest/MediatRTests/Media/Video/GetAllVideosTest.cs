using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Video.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Videos;

public class GetAllVideosTest
{
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetAllVideosTest()
    {
        this._mockRepository = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
        this._mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();

        this._mockLocalizer
                .Setup(x => x["CannotFindAnyVideos"])
                .Returns(new LocalizedString("CannotFindAnyVideos", "Cannot find any videos"));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_Type()
    {
        // Arrange
        (this._mockRepository, this._mockMapper) = MockRepoAndMapper(this._mockRepository, this._mockMapper);
        var handler = new GetAllVideosHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<VideoDTO>>(result.ValueOrDefault));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        // Arrange
        (this._mockRepository, this._mockMapper) = MockRepoAndMapper(this._mockRepository, this._mockMapper);
        var handler = new GetAllVideosHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetListVideosDTO().Count, result.Value.Count()));
    }

    [Fact]
    public async Task ShouldThrowExeption_ReppoReturnNull()
    {
        // Arrange
        this._mockRepository.Setup(x => x.VideoRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideosWithNotExistingId());

        this._mockMapper
            .Setup(x => x
            .Map<IEnumerable<VideoDTO>>(It.IsAny<IEnumerable<Video>>()))
            .Returns(GetVideosDTOWithNotExistingId());

        var expectedError = "Cannot find any videos";
        var handler = new GetAllVideosHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    private static (Mock<IRepositoryWrapper> _mockRepository, Mock<IMapper> _mockMapper) MockRepoAndMapper(
        Mock<IRepositoryWrapper> mockRepository,
        Mock<IMapper> mockMapper)
    {
        mockRepository.Setup(x => x.VideoRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetListVideos());

        mockMapper
            .Setup(x => x
            .Map<IEnumerable<VideoDTO>>(It.IsAny<IEnumerable<Video>>()))
            .Returns(GetListVideosDTO());

        return (mockRepository, mockMapper);
    }

    private static IQueryable<Video> GetListVideos()
    {
        var videos = new List<Video>
        {
            new Video
            {
                Id = 1,
            },
            new Video
            {
                Id = 2,
            },
        };

        return videos.AsQueryable();
    }

    private static List<VideoDTO> GetListVideosDTO()
    {
        var videosDTO = new List<VideoDTO>
        {
            new VideoDTO
            {
                Id = 1,
            },
            new VideoDTO
            {
                Id = 2,
            },
        };

        return videosDTO;
    }

    private static List<Video> GetVideosWithNotExistingId()
    {
        return new List<Video>();
    }

    private static List<VideoDTO> GetVideosDTOWithNotExistingId()
    {
        return new List<VideoDTO>();
    }
}
