using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Users.GetByUserName;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Users;

public class GetOtherUserByUserNameHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly Mock<IStringLocalizer<UserSharedResource>> _localizerUserSharedResourceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly GetOtherUserByUserNameHandler _handler;

    public GetOtherUserByUserNameHandlerTests()
    {
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();
        _localizerUserSharedResourceMock = new Mock<IStringLocalizer<UserSharedResource>>();

        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        _handler = new GetOtherUserByUserNameHandler(
            _mapperMock.Object,
            _repositoryWrapperMock.Object,
            _loggerMock.Object,
            _userManagerMock.Object,
            _localizerUserSharedResourceMock.Object);
    }

    [Fact]
    public async Task Handle_UserExists_ReturnsUserProfileDTO()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var userName = "testUser";
        var user = new User { UserName = userName, Id = "123" };
        var userProfileDto = new UserProfileDTO { UserName = userName, Role = "Admin" };

        _repositoryWrapperMock.Setup(repo => repo.UserRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .ReturnsAsync(user);

        _mapperMock.Setup(m => m.Map<UserProfileDTO>(It.IsAny<User>())).Returns(userProfileDto);
        _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

        // Act
        var result = await _handler.Handle(new GetOtherUserByUserNameQuery(userName), cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userName, result.Value.UserName);
        Assert.Equal("Admin", result.Value.Role);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var userName = "nonExistentUser";

        _repositoryWrapperMock.Setup(repo => repo.UserRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .ReturnsAsync((User)null!);

        _localizerUserSharedResourceMock.Setup(l => l["UserWithSuchUsernameNotExists"]).Returns(new LocalizedString("UserWithSuchUsernameNotExists", "User does not exist"));

        // Act
        var result = await _handler.Handle(new GetOtherUserByUserNameQuery(userName), cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }
}