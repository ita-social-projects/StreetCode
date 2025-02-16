using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllPublished;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetAllPublishedHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockNoSharedResourceLocalizer _mockNoSharedResourceLocalizer;
    private readonly GetAllPublishedHandler _handler;

    public GetAllPublishedHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockNoSharedResourceLocalizer = new MockNoSharedResourceLocalizer();

        _handler = new GetAllPublishedHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _mockNoSharedResourceLocalizer);
    }

    [Fact]
    public async Task Handle_WhenPublishedStreetcodesExist_ReturnsStreetcodeShortDTOs()
    {
        // Arrange
        var testStreetcodes = GetTestStreetcodes(3);
        GetAllPublishedQuery query = new GetAllPublishedQuery();

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                sc => sc.Status == StreetcodeStatus.Published, null))
            .ReturnsAsync(testStreetcodes);

        SetupMocks(testStreetcodes);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(testStreetcodes.Count, result.Value.Count());
        _repositoryMock.Verify(repo => repo.StreetcodeRepository.GetAllAsync(sc => sc.Status == StreetcodeStatus.Published, null), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<StreetcodeShortDTO>>(testStreetcodes), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoPublishedStreetcodesExist_ReturnsError()
    {
        // Arrange
        string expectedErrorKey = "NoStreetcodesExistNow";
        string expectedErrorValue = _mockNoSharedResourceLocalizer[expectedErrorKey];
        var query = new GetAllPublishedQuery();

        SetupMocks(null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        _loggerMock.Verify(logger => logger.LogError(query, expectedErrorValue), Times.Once);
    }

    private static List<StreetcodeContent> GetTestStreetcodes(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => new StreetcodeContent { Id = i, Status = StreetcodeStatus.Published })
            .ToList();
    }

    private void SetupMocks(List<StreetcodeContent>? streetcodes)
    {
        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                sc => sc.Status == StreetcodeStatus.Published, null))
            .ReturnsAsync(streetcodes!);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<StreetcodeShortDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns((IEnumerable<StreetcodeContent> src) =>
                src.Select(s => new StreetcodeShortDTO()
                {
                    Id = s.Id,
                }).ToList());
    }
}