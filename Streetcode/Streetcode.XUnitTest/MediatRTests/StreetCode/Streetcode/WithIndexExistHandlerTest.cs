using System.Linq.Expressions;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class WithIndexExistHandlerTest
{
    private readonly Mock<IRepositoryWrapper> _repository;

    public WithIndexExistHandlerTest()
    {
        _repository = new Mock<IRepositoryWrapper>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccesfully(int id)
    {
        // Arrange
        _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(GetStreetCodeContent(id));

        var handler = new StreetcodeWithIndexExistHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new StreetcodeWithIndexExistQuery(id, UserRole.User), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsAssignableFrom<Result<bool>>(result),
            () => Assert.True(result.Value));
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnFalse_NotExistingId(int id)
    {
        // Arrange
        _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(GetNull());

        var handler = new StreetcodeWithIndexExistHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new StreetcodeWithIndexExistQuery(id, UserRole.User), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsAssignableFrom<Result<bool>>(result),
            () => Assert.False(result.Value));
    }

    private static StreetcodeContent GetStreetCodeContent(int id)
    {
        return new StreetcodeContent() { Id = id };
    }

    private static StreetcodeContent? GetNull()
    {
        return null;
    }
}