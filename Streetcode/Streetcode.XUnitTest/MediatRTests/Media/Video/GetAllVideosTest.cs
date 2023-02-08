using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Media.Video.GetAll;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Videos;

public class GetAllVideosTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetAllVideosTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldReturn_SuccessfullyType()
    {
        //Arrange
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

        //Act
        var handler = new GetAllVideosHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<List<VideoDTO>>(result.ValueOrDefault);
    }

    [Fact]
    public async Task ShouldReturn_SuccessfullyCountMatch()
    {
        //Arrange
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

        //Act
        var handler = new GetAllVideosHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(GetListVideosDTO().Count, result.Value.Count());
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
}
