using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Media.Video.GetById;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Video;

public class GetVideoByIdTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetVideoByIdTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ShouldReturnSuccessfullyExistingId(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Media.Video, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Media.Video>,
                IIncludableQueryable<DAL.Entities.Media.Video, object>>>()))
            .ReturnsAsync(new DAL.Entities.Media.Video()
            {
                Id = id
            });

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<DAL.Entities.Media.Video>()))
            .Returns(new VideoDTO { Id = id });

        //Act
        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByIdQuery (id), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Id, id);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ShouldThrowErrorWhenIdNotExist(int id)
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
        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Equal($"Cannot find a video with corresponding id: {id}", result.Errors.First().Message);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ShouldReturnSuccessfullyCorrectType(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Media.Video, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Media.Video>,
                IIncludableQueryable<DAL.Entities.Media.Video, object>>>()))
            .ReturnsAsync(new DAL.Entities.Media.Video()
            {
                Id = id
            });

        _mockMapper
             .Setup(x => x
             .Map<VideoDTO>(It.IsAny<DAL.Entities.Media.Video>()))
             .Returns(new VideoDTO { Id = id });

        //Act
        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);

        //Assert
        Assert.NotNull(result.ValueOrDefault);
        Assert.IsType<VideoDTO>(result.ValueOrDefault);
    }
}
