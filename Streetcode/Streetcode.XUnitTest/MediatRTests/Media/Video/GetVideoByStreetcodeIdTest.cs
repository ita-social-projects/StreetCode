using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Videos;

public class GetVideoByStreetcodeIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

    public GetVideoByStreetcodeIdTest()
    {
        this._mockRepository = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
        this._mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_ExistingId(int streetcodeId)
    {
        // Arrange
        this._mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideo(streetcodeId));

        this._mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<Video>()))
            .Returns(GetVideoDTO(streetcodeId));

        var handler = new GetVideoByStreetcodeIdHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

        // Act
        var result = await handler.Handle(
            new GetVideoByStreetcodeIdQuery(streetcodeId),
            CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.Id, streetcodeId));
    }

    [Theory]
    [InlineData(-1)]
    public async Task ShouldThrowError_IdNotExist(int streetcodeId)
    {
        // Arrange
        Video? video = null;
        VideoDTO? videoDto = null;
        StreetcodeContent? streetcodeContent = null;
        this._mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(video);

        this._mockRepository.Setup(x => x.StreetcodeRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(streetcodeContent);

        this._mockMapper
            .Setup(x => x
            .Map<VideoDTO?>(It.IsAny<Video>()))
            .Returns(videoDto);

        this._mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int streetcodeId)
                   {
                       return new LocalizedString(key, $"Cannot find a video with corresponding id: {streetcodeId}");
                   }

                   return new LocalizedString(key, "Cannot find any video with unknown id");
               });

        var handler = new GetVideoByStreetcodeIdHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

        // Act
        var result = await handler.Handle(
            new GetVideoByStreetcodeIdQuery(streetcodeId),
            CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsFailed));
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_CorrectType(int streetcodeId)
    {
        // Arrange
        this._mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideo(streetcodeId));

        this._mockMapper
             .Setup(x => x
             .Map<VideoDTO>(It.IsAny<Video>()))
             .Returns(GetVideoDTO(streetcodeId));

        var handler = new GetVideoByStreetcodeIdHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

        // Act
        var result = await handler.Handle(
            new GetVideoByStreetcodeIdQuery(streetcodeId),
            CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result.ValueOrDefault),
            () => Assert.IsType<VideoDTO>(result.ValueOrDefault));
    }

    private static VideoDTO GetVideoDTO(int id)
    {
        return new VideoDTO()
        {
            Id = id,
        };
    }

    private static Video GetVideo(int id)
    {
        return new Video()
        {
            Id = id,
        };
    }
}