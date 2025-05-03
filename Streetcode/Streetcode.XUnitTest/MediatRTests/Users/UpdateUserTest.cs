using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Users.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Entities.Users.Expertise;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Users;
public class UpdateUserHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IStringLocalizer<UserSharedResource>> _localizerMock;
    private readonly Mock<IUserStore<User>> _userStoreMock;
    private readonly UpdateUserHandler _handler;

    public UpdateUserHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _loggerMock = new Mock<ILoggerService>();
        _userStoreMock = new Mock<IUserStore<User>>();
        _localizerMock = new Mock<IStringLocalizer<UserSharedResource>>();

        _userManagerMock = new Mock<UserManager<User>>(
            _userStoreMock.Object, null, null, null, null, null, null, null, null);

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var fakeEmail = "test@example.com";
        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, fakeEmail),
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var context = new DefaultHttpContext { User = claimsPrincipal };

        _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);

        _handler = new UpdateUserHandler(
            _mapperMock.Object,
            _repositoryWrapperMock.Object,
            _loggerMock.Object,
            _userManagerMock.Object,
            _httpContextAccessorMock.Object,
            _localizerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUpdatedUser_WhenValidRequestIsProvided()
    {
        // Arrange
        var existingUser = GetExistingUser();

        var updateUserCommand = GetUpdateUserCommand();

        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<User> { existingUser }.AsQueryable().BuildMockDbSet().Object);

        _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        _repositoryWrapperMock.Setup(rw => rw.ExpertiseRepository.GetAllAsync(
                It.IsAny<Expression<Func<Expertise, bool>>>(),
                It.IsAny<Func<IQueryable<Expertise>, IIncludableQueryable<Expertise, object>>>()))
            .ReturnsAsync(new List<Expertise> { new () { Id = 2, Title = "New Expertise" } });

        _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Admin" });

        _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
            .Returns(GetUserDto);

        // Act
        var result = await _handler.Handle(updateUserCommand, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("newusername", result.Value.UserName);
        Assert.Equal("NewName", result.Value.Name);
        Assert.Contains(result.Value.Expertises, e => e.Title == "New Expertise");
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenUpdateAsyncFails()
    {
        // Arrange
        var existingUser = GetExistingUser();

        var updateUserCommand = GetUpdateUserCommand();

        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<User> { existingUser }.AsQueryable().BuildMockDbSet().Object);

        _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));

        _repositoryWrapperMock.Setup(rw => rw.ExpertiseRepository.GetAllAsync(
                It.IsAny<Expression<Func<Expertise, bool>>>(),
                It.IsAny<Func<IQueryable<Expertise>, IIncludableQueryable<Expertise, object>>>()))
            .ReturnsAsync(new List<Expertise> { new () { Id = 2, Title = "New Expertise" } });

        _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Admin" });

        _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
            .Returns(GetUserDto);

        // Act
        var result = await _handler.Handle(updateUserCommand, default);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserIsNotFound()
    {
        // Arrange
        var updateUserCommand = GetUpdateUserCommand();

        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<User>().AsQueryable().BuildMockDbSet().Object);

        _repositoryWrapperMock.Setup(rw => rw.ExpertiseRepository.GetAllAsync(
                It.IsAny<Expression<Func<Expertise, bool>>>(),
                It.IsAny<Func<IQueryable<Expertise>, IIncludableQueryable<Expertise, object>>>()))
            .ReturnsAsync(new List<Expertise> { new () { Id = 2, Title = "New Expertise" } });

        _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
            .Returns(GetUserDto);

        // Act
        var result = await _handler.Handle(updateUserCommand, default);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionOccurs()
    {
        // Arrange
        var updateUserCommand = GetUpdateUserCommand();

        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<User>().AsQueryable().BuildMockDbSet().Object);

        _repositoryWrapperMock.Setup(rw => rw.ExpertiseRepository.GetAllAsync(
                It.IsAny<Expression<Func<Expertise, bool>>>(),
                It.IsAny<Func<IQueryable<Expertise>, IIncludableQueryable<Expertise, object>>>()))
            .ReturnsAsync(new List<Expertise> { new () { Id = 2, Title = "New Expertise" } });

        _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Database fail"));

        _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
            .Returns(GetUserDto);

        // Act
        var result = await _handler.Handle(updateUserCommand, default);

        // Assert
        Assert.False(result.IsSuccess);
    }

    private static UpdateUserCommand GetUpdateUserCommand()
    {
        return new UpdateUserCommand(
            new UpdateUserDTO
            {
                Id = "D897C7AE-B6C1-4E01-95B8-D35ECD49369D",
                UserName = "newusername",
                Name = "New Name",
                Surname = "New Surname",
                AvatarId = 2,
                AboutYourself = "About Myself",
                PhoneNumber = "1234567890",
                Expertises = new List<ExpertiseDTO>
                {
                    new () { Id = 2, Title = "New Expertise" },
                },
            });
    }

    private static User GetExistingUser()
    {
        return new User
        {
            Id = "D897C7AE-B6C1-4E01-95B8-D35ECD49369D",
            UserName = "oldusername",
            Name = "Old Name",
            Surname = "Old Surname",
            Email = "test@example.com",
            Expertises = new List<Expertise>
            {
                new () { Id = 1, Title = "Old Expertise" },
            },
        };
    }

    private static UserDTO GetUserDto(User user)
    {
        return new UserDTO
        {
            UserName = user.UserName,
            Name = user.Name,
            Surname = user.Surname,
            AvatarId = user.AvatarId,
            AboutYourself = user.AboutYourself,
            PhoneNumber = user.PhoneNumber!,
            Expertises = user.Expertises.Select(e => new ExpertiseDTO { Id = e.Id, Title = e.Title }).ToList(),
        };
    }
}