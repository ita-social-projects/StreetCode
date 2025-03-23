using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Video.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Videos;

public class GetVideoByIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

    public GetVideoByIdTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturn_SuccessfullyExistingId(int id)
    {
        // Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideo(id));

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO>(It.IsAny<Video>()))
            .Returns(GetVideoDto(id));

        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(
            new GetVideoByIdQuery(id),
            CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.Id, id));
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldThrowError_WhenIdNotExist(int id)
    {
        // Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideoWithNotExistingId());

        _mockMapper
            .Setup(x => x
            .Map<VideoDTO?>(It.IsAny<Video>()))
            .Returns(GetVideoDtoWithNotExistingId());

        var expectedError = $"Cannot find a video with corresponding id: {id}";
        _mockLocalizer
            .Setup(x => x["CannotFindVideoWithCorrespondingId", id])
            .Returns(new LocalizedString("CannotFindVideoWithCorrespondingId", expectedError));

        // Act
        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        var result = await handler.Handle(
            new GetVideoByIdQuery(id),
            CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(expectedError, result.Errors[0].Message));
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_CorrectType(int id)
    {
        // Arrange
        _mockRepository.Setup(x => x.VideoRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Video, bool>>>(),
                It.IsAny<Func<IQueryable<Video>,
                IIncludableQueryable<Video, object>>>()))
            .ReturnsAsync(GetVideo(id));

        _mockMapper
             .Setup(x => x
             .Map<VideoDTO>(It.IsAny<Video>()))
             .Returns(GetVideoDto(id));

        // Act
        var handler = new GetVideoByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        var result = await handler.Handle(
            new GetVideoByIdQuery(id),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result.ValueOrDefault);
        Assert.IsType<VideoDTO>(result.ValueOrDefault);
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

    private static VideoDTO? GetVideoDtoWithNotExistingId()
    {
        return null;
    }

    private static Video? GetVideoWithNotExistingId()
    {
        return null;
    }
}
