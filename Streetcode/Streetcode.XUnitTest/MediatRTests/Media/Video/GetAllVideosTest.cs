using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Video.GetAll;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Videos;

public class GetAllVideosTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    public GetAllVideosTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_Type()
    {
        //Arrange
        (_mockRepository, _mockMapper) = MockRepoAndMapper(_mockRepository, _mockMapper);
        var handler = new GetAllVideosHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<VideoDTO>>(result.ValueOrDefault)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        //Arrange
        (_mockRepository, _mockMapper) = MockRepoAndMapper(_mockRepository, _mockMapper);    
        var handler = new GetAllVideosHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetListVideosDTO().Count, result.Value.Count())
        );
    }

    [Fact]
    public async Task ShouldThrowExeption_ReppoReturnNull()
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<Video, bool>>>(),
                    It.IsAny<Func<IQueryable<Video>,
              IIncludableQueryable<Video, object>>>()))
              .ReturnsAsync(GetVideosWithNotExistingId());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<VideoDTO>>(It.IsAny<IEnumerable<Video>>()))
            .Returns(GetVideosDTOWithNotExistingId());

        var expectedError = "Cannot find any videos";
        var handler = new GetAllVideosHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);
    }

    private static (Mock<IRepositoryWrapper> _mockRepository, Mock<IMapper>  _mockMapper) MockRepoAndMapper(
        Mock<IRepositoryWrapper> _mockRepository,
        Mock<IMapper> _mockMapper) 
    {
        _mockRepository.Setup(x => x.VideoRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<Video, bool>>>(),
                    It.IsAny<Func<IQueryable<Video>,
              IIncludableQueryable<Video, object>>>()))
              .ReturnsAsync(GetListVideos());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<VideoDTO>>(It.IsAny<IEnumerable<Video>>()))
            .Returns(GetListVideosDTO());

        return (_mockRepository, _mockMapper);
    }

    private static IQueryable<Video> GetListVideos()
    {
        var videos = new List<Video>
        {
            new Video
            {
                Id = 1
            },

            new Video
            {
                Id = 2
            }
        };

        return videos.AsQueryable();
    }

    private static List<VideoDTO> GetListVideosDTO()
    {
        var videosDTO = new List<VideoDTO>
        {
             new VideoDTO
            {
                Id = 1
            },

            new VideoDTO
            {
                Id = 2
            }
        };

        return videosDTO;
    }
    private static List<Video>? GetVideosWithNotExistingId()
    {
        return null;
    }
    private static List<VideoDTO>? GetVideosDTOWithNotExistingId()
    {
        return null;
    }
}
