using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.Factories.MessageDataFactory.Abstracts;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Authentication.Register;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.XUnitTest.MediatRTests.Authentication.Register;

using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

public class HandleRegisterTest
{
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<IRepositoryWrapper> mockRepositoryWrapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<UserManager<User>> mockUserManager;
    private readonly Mock<IStringLocalizer<UserSharedResource>> mockLocalizer;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IMessageDataAbstractFactory> _messageDataAbstractFactoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextMockAccessorMock;

    public HandleRegisterTest()
    {
        mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        mockMapper = new Mock<IMapper>();
        mockLogger = new Mock<ILoggerService>();
        _emailServiceMock = new Mock<IEmailService>();
        _messageDataAbstractFactoryMock = new Mock<IMessageDataAbstractFactory>();
        _httpContextMockAccessorMock = new Mock<IHttpContextAccessor>();

        var store = new Mock<IUserStore<User>>();
        mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockLocalizer = new Mock<IStringLocalizer<UserSharedResource>>();

        mockLocalizer.Setup(x => x["UserWithSuchEmailExists"]).Returns(new LocalizedString("UserWithSuchEmailExists", "UserWithSuchEmailExists"));
        mockLocalizer.Setup(x => x["UserWithSuchUsernameExists"]).Returns(new LocalizedString("UserWithSuchUsernameExists", "UserWithSuchUsernameExists"));
        mockLocalizer.Setup(x => x["UserManagerError"]).Returns(new LocalizedString("UserManagerError", "UserManagerError"));
    }

    [Fact]
    public async Task ShouldReturnSuccess_NewUser()
    {
        // Arrange.
        SetupServicesForSuccess();
        SetupMockMapper();
        SetupMockUserManagerGenerateEmailConfirmationTokenAsync();
        var handler = GetRegisterHandler();

        // Act.
        var result = await handler.Handle(new RegisterQuery(GetSampleRequestDTO()), CancellationToken.None);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldReturnRegisterResponseDTOWithCorrectData_NewUser()
    {
        // Arrange.
        User expectedUser = GetSampleUser();
        string expectedRole = nameof(UserRole.User);
        SetupServicesForSuccess();
        SetupMockMapper(expectedUser);
        SetupMockUserManagerGenerateEmailConfirmationTokenAsync();
        var handler = GetRegisterHandler();

        // Act.
        var result = await handler.Handle(new RegisterQuery(GetSampleRequestDTO()), CancellationToken.None);

        // Assert.
        Assert.Multiple(
            () => Assert.Equal(expectedUser.Id, result.Value.Id),
            () => Assert.Equal(expectedUser.Name, result.Value.Name),
            () => Assert.Equal(expectedUser.Surname, result.Value.Surname),
            () => Assert.Equal(expectedUser.Email, result.Value.Email),
            () => Assert.Equal(expectedUser.UserName, result.Value.UserName),
            () => Assert.Equal(expectedRole, result.Value.Role));
    }

    [Fact]
    public async Task ShouldReturnFailWithCorrectMessage_UserWithGivenEmailIsAlreadyInDatabase()
    {
        // Arrange.
        string expectedErrorMessage = "UserWithSuchEmailExists";
        SetupMockRepositoryGetFirstOrDefault(isExists: true);
        SetupMockMapper(isEmailExists: true);
        var handler = GetRegisterHandler();

        // Act.
        var result = await handler.Handle(new RegisterQuery(GetRegisterRequestWithExistingEmail()), CancellationToken.None);

        // Assert.
        Assert.True(result.IsFailed);
        Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldReturnFailWithCorrectMessage_UserWithGivenUserNameIsAlreadyInDatabase()
    {
        // Arrange.
        string expectedErrorMessage = "UserWithSuchUsernameExists";
        SetupMockRepositoryGetFirstOrDefault(isExists: true);
        SetupMockMapper();
        var handler = GetRegisterHandler();

        // Act.
        var result = await handler.Handle(new RegisterQuery(GetRegisterRequestWithExistingUserName()), CancellationToken.None);

        // Assert.
        Assert.True(result.IsFailed);
        Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldReturnFailWithCorrectMessage_CreateUserFails()
    {
        // Arrange.
        string expectedErrorMessage = "UserManagerError";
        SetupMockRepositoryGetFirstOrDefault(isExists: false);
        SetupMockUserManagerCreate(isSuccess: false);
        SetupMockMapper();
        var handler = GetRegisterHandler();

        // Act.
        var result = await handler.Handle(new RegisterQuery(GetSampleRequestDTO()), CancellationToken.None);

        // Assert.
        Assert.True(result.IsFailed);
        Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldReturnFailWithCorrectMessage_ExceptionIsThrownByCreateUser()
    {
        // Arrange.
        string expectedErrorMessage = "Exception is thrown from UserManager while creating user";
        SetupMockRepositoryGetFirstOrDefault(isExists: false);
        SetupMockUserManagerCreateThrowsException(expectedErrorMessage);
        SetupMockMapper();
        var handler = GetRegisterHandler();

        // Act.
        var result = await handler.Handle(new RegisterQuery(GetSampleRequestDTO()), CancellationToken.None);

        // Assert.
        Assert.True(result.IsFailed);
        Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldReturnFailWithCorrectMessage_ExceptionIsThrownByAddToRoleAsync()
    {
        // Arrange.
        string expectedErrorMessage = "Exception is thrown from UserManager while assigning role to user";
        SetupMockRepositoryGetFirstOrDefault(isExists: false);
        SetupMockUserManagerCreate(isSuccess: true);
        SetupMockUserManagerAddToRoleThrowsException(expectedErrorMessage);
        SetupMockMapper();
        var handler = GetRegisterHandler();

        // Act.
        var result = await handler.Handle(new RegisterQuery(GetSampleRequestDTO()), CancellationToken.None);

        // Assert.
        Assert.True(result.IsFailed);
        Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
    }

    private User GetSampleUser()
    {
        return new User()
        {
            Id = Guid.NewGuid().ToString(),
            Email = "zero@gmail.com",
            UserName = "Zero_zero",
            Name = "SampleName",
            Surname = "SampleSurname",
        };
    }

    private RegisterRequestDTO GetSampleRequestDTO()
    {
        return new RegisterRequestDTO()
        {
            Email = "zero@gmail.com",
        };
    }

    private RegisterRequestDTO GetRegisterRequestWithExistingEmail()
    {
        return new RegisterRequestDTO()
        {
            Email = "zero@gmail.com",
        };
    }

    private RegisterRequestDTO GetRegisterRequestWithExistingUserName()
    {
        return new RegisterRequestDTO()
        {
            Email = string.Empty,
        };
    }

    private void SetupServicesForSuccess()
    {
        SetupMockRepositoryGetFirstOrDefault(isExists: false);
        SetupMockUserManagerCreate(isSuccess: true);
    }

    private void SetupMockRepositoryGetFirstOrDefault(bool isExists)
    {
        mockRepositoryWrapper
            .Setup(wrapper => wrapper.UserRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>,
                        IIncludableQueryable<User, object>>>()))
            .ReturnsAsync(isExists ? GetSampleUser() : null);
    }

    private void SetupMockMapper(User? user = null, bool isEmailExists = false)
    {
        RegisterResponseDTO registerResponseToReturnFromMapper = new RegisterResponseDTO();
        if (user is not null)
        {
            registerResponseToReturnFromMapper = new RegisterResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email!,
                UserName = user.UserName!,
            };
        }

        mockMapper
            .Setup(x => x
                .Map<RegisterResponseDTO>(It.IsAny<User>()))
            .Returns(registerResponseToReturnFromMapper);

        User sampleUser = GetSampleUser();
        if (isEmailExists)
        {
            sampleUser.UserName = string.Empty;
        }
        else
        {
            sampleUser.Email = string.Empty;
        }

        mockMapper
            .Setup(x => x
                .Map<User>(It.IsAny<RegisterRequestDTO>()))
            .Returns(sampleUser);
    }

    private void SetupMockUserManagerCreate(bool isSuccess)
    {
        mockUserManager
            .Setup(manager => manager
                .CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(isSuccess ? IdentityResult.Success : IdentityResult.Failed());
    }

    private void SetupMockUserManagerGenerateEmailConfirmationTokenAsync()
    {
        mockUserManager
            .Setup(manager => manager
                .GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
            .ReturnsAsync("Test_Token");
    }

    private void SetupMockUserManagerCreateThrowsException(string message)
    {
        mockUserManager
            .Setup(manager => manager
                .CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception(message));
    }

    private void SetupMockUserManagerAddToRoleThrowsException(string message)
    {
        mockUserManager
            .Setup(manager => manager
                .AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception(message));
    }

    private RegisterHandler GetRegisterHandler()
    {
        return new RegisterHandler(
            mockRepositoryWrapper.Object,
            mockLogger.Object,
            mockMapper.Object,
            mockUserManager.Object,
            mockLocalizer.Object,
            _emailServiceMock.Object,
            _messageDataAbstractFactoryMock.Object,
            _httpContextMockAccessorMock.Object);
    }
}