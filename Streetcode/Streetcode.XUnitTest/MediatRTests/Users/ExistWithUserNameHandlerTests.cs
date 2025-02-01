using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.MediatR.Users.GetAllUserName;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Users;

public class ExistWithUserNameHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly ExistWithUserNameHandler _handler;

    public ExistWithUserNameHandlerTests()
    {
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _handler = new ExistWithUserNameHandler(_repositoryWrapperMock.Object);
    }

    [Fact]
    public async Task Handle_UserExists_ReturnsTrue()
    {
        // Arrange
        var userName = "testUser";
        var user = new User { UserName = userName };

        _repositoryWrapperMock.Setup(repo => repo.UserRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<User, bool>>>(), null))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(new ExistWithUserNameQuery(userName), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var userName = "nonExistentUser";

        _repositoryWrapperMock.Setup(repo => repo.UserRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<User, bool>>>(), null))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _handler.Handle(new ExistWithUserNameQuery(userName), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
    }
}