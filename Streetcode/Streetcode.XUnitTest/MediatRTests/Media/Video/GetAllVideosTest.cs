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
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;
    private Mock<IRepositoryWrapper> mockRepository;
    private Mock<IMapper> mockMapper;

    public GetAllVideosTest()
    {
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();

        this.mockLocalizer
                .Setup(x => x["CannotFindAnyVideos"])
                .Returns(new LocalizedString("CannotFindAnyVideos", "Cannot find any videos"));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_Type()
    {
        // Arrange
        (this.mockRepository, this.mockMapper) = MockRepoAndMapper(this.mockRepository, this.mockMapper);
        var handler = new GetAllVideosHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<VideoDto>>(result.ValueOrDefault));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        // Arrange
        (this.mockRepository, this.mockMapper) = MockRepoAndMapper(this.mockRepository, this.mockMapper);
        var handler = new GetAllVideosHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

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
        this.mockRepository.Setup(x => x.VideoRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideosWithNotExistingId());

        this.mockMapper
            .Setup(x => x
            .Map<IEnumerable<VideoDto>>(It.IsAny<IEnumerable<Video>>()))
            .Returns(GetVideosDTOWithNotExistingId());

        var expectedError = "Cannot find any videos";
        var handler = new GetAllVideosHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

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
            .Map<IEnumerable<VideoDto>>(It.IsAny<IEnumerable<Video>>()))
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

    private static List<VideoDto> GetListVideosDTO()
    {
        var videosDTO = new List<VideoDto>
        {
            new VideoDto
            {
                Id = 1,
            },
            new VideoDto
            {
                Id = 2,
            },
        };

        return videosDTO;
    }

    private static List<Video> GetVideosWithNotExistingId()
    {
        return null;
    }

    private static List<VideoDto> GetVideosDTOWithNotExistingId()
    {
        return null;
    }
}
