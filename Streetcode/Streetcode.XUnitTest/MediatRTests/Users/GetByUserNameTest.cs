using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Users.GetByName;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Users;

public class GetByUserNameTest
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MockUserSharedResourceLocalizer _localizerMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly GetByUserNameHandler _handler;

    public GetByUserNameTest()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _localizerMock = new MockUserSharedResourceLocalizer();
        _loggerMock = new Mock<ILoggerService>();

        var fakeEmail = "test@example.com";
        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, fakeEmail),
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var context = new DefaultHttpContext { User = claimsPrincipal };

        _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);

        _handler = new GetByUserNameHandler(
            _mapperMock.Object,
            _repositoryWrapperMock.Object,
            _loggerMock.Object,
            _userManagerMock.Object,
            _httpContextAccessorMock.Object,
            _localizerMock);
    }

    [Fact]
    public async Task ShouldReturnPaginatedNews_CorrectPage()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var userEmail = "test@example.com";
        var user = new User
        {
            Email = userEmail,
            Id = "123",
        };
        var userDto = new UserDTO
        {
            Email = userEmail,
            Role = "Admin",
        };

        _repositoryWrapperMock.Setup(repo => repo.UserRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .ReturnsAsync(user);

        _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns(userDto);
        _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

        // Act
        var result = await _handler.Handle(new GetByUserNameQuery(), cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userEmail, result.Value.Email);
        Assert.Equal("Admin", result.Value.Role);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        _repositoryWrapperMock.Setup(repo => repo.UserRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _handler.Handle(new GetByUserNameQuery(), cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }
}