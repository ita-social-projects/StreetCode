using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.WithUrlExist;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class StreetcodeWithUrlExistHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly StreetcodeWithUrlExistHandler _handler;

    public StreetcodeWithUrlExistHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();

        _handler = new StreetcodeWithUrlExistHandler(
            _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenStreetcodeExists_ReturnsTrue()
    {
        // Arrange
        var request = new StreetcodeWithUrlExistQuery(url: "existing-url");
        var testStreetcode = new StreetcodeContent { TransliterationUrl = "existing-url" };

        _repositoryMock.Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(testStreetcode);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Handle_WhenStreetcodeDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var request = new StreetcodeWithUrlExistQuery(url: "non-existing-url");

        _repositoryMock.Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync((StreetcodeContent?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
    }
}