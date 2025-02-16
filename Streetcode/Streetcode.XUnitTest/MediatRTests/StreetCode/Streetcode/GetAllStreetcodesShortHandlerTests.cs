using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetAllStreetcodesShortHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockNoSharedResourceLocalizer _mockLocalizer;
    private readonly GetAllStreetcodesShortHandler _handler;

    public GetAllStreetcodesShortHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockLocalizer = new MockNoSharedResourceLocalizer();

        _handler = new GetAllStreetcodesShortHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _mockLocalizer);
    }

    [Fact]
    public async Task Handle_WhenStreetcodesExist_ReturnsStreetcodeShortDTOs()
    {
        // Arrange
        var testStreetcodes = GetTestStreetcodes(3);

        SetupRepositoryMock(testStreetcodes);

        // Act
        var result = await _handler.Handle(new GetAllStreetcodesShortQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(testStreetcodes.Count, result.Value.Count());
            _repositoryMock.Verify(repo => repo.StreetcodeRepository.GetAllAsync(null, null), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<StreetcodeShortDTO>>(testStreetcodes), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenNoStreetcodesExist_ReturnsError()
    {
        // Arrange
        const string expectedErrorKey = "NoStreetcodesExistNow";
        string expectedErrorValue = _mockLocalizer[expectedErrorKey];
        var query = new GetAllStreetcodesShortQuery();

        SetupRepositoryMock(new List<StreetcodeContent>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
            _loggerMock.Verify(logger => logger.LogError(query, expectedErrorValue), Times.Once);
        });
    }

    private static List<StreetcodeContent> GetTestStreetcodes(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => new StreetcodeContent { Id = i })
            .ToList();
    }

    private void SetupRepositoryMock(List<StreetcodeContent>? streetcodes)
    {
        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetAllAsync(null, null))
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