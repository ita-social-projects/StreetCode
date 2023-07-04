using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Videos;

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
    public async Task ShouldReturnSuccessfully_ExistingId(int streetcodeId)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideo(streetcodeId));

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<Video>()))
            .Returns(GetVideoDTO(streetcodeId));

        var handler = new GetVideoByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetVideoByStreetcodeIdQuery(streetcodeId),
            CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.Id, streetcodeId)
        );
    }

    [Theory]
    [InlineData(-1)]
    public async Task ShouldThrowError_IdNotExist(int streetcodeId)
    {
        //Arrange

        Video video  = null;
        VideoDTO videoDto = null;
        StreetcodeContent streetcodeContent = null;
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(video);

        _mockRepository.Setup(x => x.StreetcodeRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(streetcodeContent);

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<Video>()))
            .Returns(videoDto);

        var handler = new GetVideoByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetVideoByStreetcodeIdQuery(streetcodeId),
            CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsFailed)
        );
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_CorrectType(int streetcodeId)
    {
        //Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideo(streetcodeId));

        _mockMapper
             .Setup(x => x
             .Map<VideoDTO>(It.IsAny<Video>()))
             .Returns(GetVideoDTO(streetcodeId));

        var handler = new GetVideoByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetVideoByStreetcodeIdQuery(streetcodeId),
            CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result.ValueOrDefault),
            () => Assert.IsType<VideoDTO>(result.ValueOrDefault)
        );
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