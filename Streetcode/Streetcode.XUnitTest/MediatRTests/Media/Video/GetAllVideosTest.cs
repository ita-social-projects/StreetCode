using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Media.Video.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Video;

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
    public async Task GetAllFacts_ShouldReturnSuccessfullyType()
    {
        (_mockMapper, _mockRepository) = GetMapperAndRepo(_mockMapper, _mockRepository);

        var handler = new GetAllVideosHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.IsType<List<VideoDTO>>(result.ValueOrDefault);
    }

    [Fact]
    public async Task GetAllFacts_ShouldReturnSuccessfullyCountMatch()
    {
        (_mockMapper, _mockRepository) = GetMapperAndRepo(_mockMapper, _mockRepository);

        var handler = new GetAllVideosHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetAllVideosQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(GetListVideosDTO().Count, result.Value.Count());
    }

    private static (Mock<IMapper>, Mock<IRepositoryWrapper>) GetMapperAndRepo(Mock<IMapper> injectedMapper, Mock<IRepositoryWrapper> injectedReppo)
    {
        injectedReppo.Setup(x => x.VideoRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<DAL.Entities.Media.Video, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Media.Video>,
              IIncludableQueryable<DAL.Entities.Media.Video, object>>>()))
              .ReturnsAsync(GetListVideos());

        injectedMapper
            .Setup(x => x
            .Map<IEnumerable<VideoDTO>>(It.IsAny<IEnumerable<DAL.Entities.Media.Video>>()))
            .Returns(GetListVideosDTO());

        return (injectedMapper, injectedReppo);
    }
    private static IQueryable<DAL.Entities.Media.Video> GetListVideos()
    {
        var videos = new List<DAL.Entities.Media.Video>
        {
            new DAL.Entities.Media.Video
            {
                Id = 1
            },

            new DAL.Entities.Media.Video
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
