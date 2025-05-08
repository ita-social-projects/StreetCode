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
using Streetcode.DAL.Enums;
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
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_ExistingId(int streetcodeId)
    {
        // Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideo(streetcodeId));

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<Video>()))
            .Returns(GetVideoDto(streetcodeId));

        var handler = new GetVideoByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(
            new GetVideoByStreetcodeIdQuery(streetcodeId, UserRole.User),
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
            .Map<VideoDTO?>(It.IsAny<Video>()))
            .Returns(videoDto);

        _mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int streetcodeId)
                   {
                       return new LocalizedString(key, $"Cannot find a video with corresponding id: {streetcodeId}");
                   }

                   return new LocalizedString(key, "Cannot find any video with unknown id");
               });

        var handler = new GetVideoByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(
            new GetVideoByStreetcodeIdQuery(streetcodeId, UserRole.User),
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
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideo(streetcodeId));

        _mockMapper
             .Setup(x => x
             .Map<VideoDTO>(It.IsAny<Video>()))
             .Returns(GetVideoDto(streetcodeId));

        var handler = new GetVideoByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(
            new GetVideoByStreetcodeIdQuery(streetcodeId, UserRole.User),
            CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result.ValueOrDefault),
            () => Assert.IsType<VideoDTO>(result.ValueOrDefault));
    }

    private static VideoDTO GetVideoDto(int id)
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