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
            .ReturnsAsync(GetVideo(id));

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<Video>()))
            .Returns(GetVideoDTO(id));

        //Act
        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByIdQuery (id),
            CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.Id, id)
        );
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
            .ReturnsAsync(GetVideoWithNotExistingId());

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<Video>()))
            .Returns(GetVideoDTOWithNotExistingId());

        var expectedError = $"Cannot find a video with corresponding id: {id}";

        //Act
        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByIdQuery(id),
            CancellationToken.None);

        //Assert

        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(expectedError, result.Errors.First().Message)
        );
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
            .ReturnsAsync(GetVideo(id));

        _mockMapper
             .Setup(x => x
             .Map<VideoDTO>(It.IsAny<Video>()))
             .Returns(GetVideoDTO(id));

        //Act
        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetVideoByIdQuery(id),
            CancellationToken.None);

        //Assert
        Assert.NotNull(result.ValueOrDefault);
        Assert.IsType<VideoDTO>(result.ValueOrDefault);
    }

    private static VideoDTO GetVideoDTO(int id)
    {
        return new VideoDTO()
        {
            Id = id
        };
    }
    private static Video GetVideo(int id)
    {
        return new Video()
        {
            Id = id
        };
    }
    private static VideoDTO? GetVideoDTOWithNotExistingId()
    {
        return null;
    }
    private static Video? GetVideoWithNotExistingId()
    {
        return null;
    }
}
