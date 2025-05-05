using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetShortById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetStreetcodeShortByIdHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockCannotMapLocalizer _mockCannotMapLocalizer;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
    private readonly GetStreetcodeShortByIdHandler _handler;

    public GetStreetcodeShortByIdHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockCannotMapLocalizer = new MockCannotMapLocalizer();
        _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        _handler = new GetStreetcodeShortByIdHandler(
            _mapperMock.Object,
            _repositoryMock.Object,
            _loggerMock.Object,
            _mockCannotMapLocalizer,
            _mockLocalizerCannotFind.Object);
    }

    [Fact]
    public async Task Handle_WhenStreetcodeExists_ReturnsStreetcodeShortDTO()
    {
        // Arrange
        var request = new GetStreetcodeShortByIdQuery(Id: 1, UserRole.User);
        var testStreetcode = new StreetcodeContent { Id = request.Id };
        var expectedDto = new StreetcodeShortDto { Id = request.Id };

        SetupRepositoryMock(testStreetcode);
        SetupMapperMock();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedDto.Id, result.Value.Id);
        });
    }

    [Fact]
    public async Task Handle_WhenMappingFails_ReturnsError()
    {
        // Arrange
        var request = new GetStreetcodeShortByIdQuery(Id: 1, UserRole.User);
        var testStreetcode = new StreetcodeContent { Id = request.Id };
        const string expectedErrorKey = "CannotMapStreetcodeToShortDTO";
        string expectedErrorValue = _mockCannotMapLocalizer[expectedErrorKey];

        SetupRepositoryMock(testStreetcode);
        _mapperMock.Setup(m => m.Map<StreetcodeShortDto>(It.IsAny<StreetcodeContent>())).Returns((StreetcodeShortDto)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
            _loggerMock.Verify(logger => logger.LogError(request, expectedErrorValue), Times.Once);
        });
    }

    private void SetupRepositoryMock(StreetcodeContent? streetcode)
    {
        _repositoryMock.Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(streetcode);
    }

    private void SetupMapperMock()
    {
        _mapperMock
            .Setup(m => m.Map<StreetcodeShortDto>(It.IsAny<StreetcodeContent>()))
            .Returns((StreetcodeContent src) => new StreetcodeShortDto { Id = src.Id });
    }
}
