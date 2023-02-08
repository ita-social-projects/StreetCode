using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Media.Video.GetById;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Videos;

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
    public async Task ShouldReturn_SuccessfullyExistingId(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(new Video()
            {
                Id = id
            });

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<Video>()))
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
    public async Task ShouldThrowError_WhenIdNotExist(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync((Video)null);

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<Video>()))
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
    public async Task ShouldReturnSuccessfully_CorrectType(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(new Video()
            {
                Id = id
            });

        _mockMapper
             .Setup(x => x
             .Map<VideoDTO>(It.IsAny<Video>()))
             .Returns(new VideoDTO { Id = id });

        //Act
        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);

        //Assert
        Assert.NotNull(result.ValueOrDefault);
        Assert.IsType<VideoDTO>(result.ValueOrDefault);
    }
}
