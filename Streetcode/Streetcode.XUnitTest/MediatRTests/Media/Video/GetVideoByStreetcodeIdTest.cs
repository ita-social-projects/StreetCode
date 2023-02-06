using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Video;

public class GetVideoByStreetcodeIdTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetVideoByStreetcodeIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ShouldReturnSuccessfullyExistingId(int streetcodeId)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Media.Video, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Media.Video>,
                IIncludableQueryable<DAL.Entities.Media.Video, object>>>()))
            .ReturnsAsync(new DAL.Entities.Media.Video()
            {
                Id = streetcodeId
            });

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<DAL.Entities.Media.Video>()))
            .Returns(new VideoDTO { Id = streetcodeId });

        //Act
        var handler = new GetVideoByStreetcodeIdQueryHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Id, streetcodeId);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ShouldThrowErrorWhenIdNotExist(int streetcodeId)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Media.Video, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Media.Video>,
                IIncludableQueryable<DAL.Entities.Media.Video, object>>>()))
            .ReturnsAsync((DAL.Entities.Media.Video)null);

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<DAL.Entities.Media.Video>()))
            .Returns((DAL.Entities.Streetcode.TextContent.Fact video) =>
            {
                return new VideoDTO { Id = video.Id };
            });

        //Act
        var handler = new GetVideoByStreetcodeIdQueryHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Equal($"Cannot find a video by a streetcode id: {streetcodeId}", result.Errors.First().Message);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ShouldReturnSuccessfullyCorrectType(int streetcodeId)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Media.Video, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Media.Video>,
                IIncludableQueryable<DAL.Entities.Media.Video, object>>>()))
            .ReturnsAsync(new DAL.Entities.Media.Video()
            {
                Id = streetcodeId
            });

        _mockMapper
             .Setup(x => x
             .Map<VideoDTO>(It.IsAny<DAL.Entities.Media.Video>()))
             .Returns(new VideoDTO { Id = streetcodeId });

        //Act
        var handler = new GetVideoByStreetcodeIdQueryHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        //Assert
        Assert.NotNull(result.ValueOrDefault);
        Assert.IsType<VideoDTO>(result.ValueOrDefault);
    }
}
