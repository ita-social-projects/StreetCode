using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.BLL.DTO.Users.Password;
using Streetcode.BLL.Models.Email.Messages.Base;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Entities.Users.Expertise;
using Streetcode.DAL.Enums;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Users;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Users;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Expertises;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Users;

[Collection("Authorization")]
public class UserControllerTests : BaseAuthorizationControllerTests<UserClient>, IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly User _testUser;
    private readonly Expertise _testExpertise;
    private readonly string _testPassword;
    private readonly UserManager<User> _userManager;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public UserControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/Users", tokenStorage)
    {
        _factory = factory;
        var serviceProvider = _factory.Services.CreateScope().ServiceProvider;

        _userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        TokenStorage = new TokenStorage();

        var uniqueExpertiseId = UniqueNumberGenerator.GenerateInt();

        _testExpertise = ExpertiseExtracter.Extract(uniqueExpertiseId);

        (_testUser, _testPassword) = UserExtracter.Extract(
            userId: Guid.NewGuid().ToString(),
            userName: Guid.NewGuid().ToString(),
            password: GenerateTestPassword(),
            nameof(UserRole.User),
            nameof(UserRole.Admin));
    }

    [Fact]
    public async Task GetByEmail_ValidToken_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await Client.GetByEmail(TokenStorage.UserAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<UserDTO>(response.Content);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task ExistWithUserName_ValidUserName_ReturnsSuccessStatusCode()
    {
        // Arrange
        var validUserName = _testUser.UserName;

        // Act
        var response = await Client.ExistWithUserName(validUserName, TokenStorage.UserAccessToken);
        bool.TryParse(response.Content, out bool result);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.True(result);
    }

    [Fact]
    public async Task ExistWithUserName_InvalidToken_ReturnsUnauthorized()
    {
        // Arrange
        var validUserName = _testUser.UserName;

        // Act
        var response = await Client.ExistWithUserName(validUserName);
        bool.TryParse(response.Content, out bool result);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.False(result);
    }

    [Fact]
    public async Task ExistWithUserName_InvalidUserName_ReturnFalse()
    {
        // Arrange
        var invalidUserName = _testUser.UserName;

        // Act
        var response = await Client.ExistWithUserName(invalidUserName.Substring(0, 4), TokenStorage.UserAccessToken);
        bool.TryParse(response.Content, out bool result);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetOtherUserByUserName_ValidUserName_ReturnsSuccessStatusCode()
    {
        // Arrange
        var validUserName = _testUser.UserName;

        // Act
        var response = await Client.GetOtherUserByUserName(validUserName);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<UserProfileDTO>(response.Content);

        // Assert
        Assert.NotNull(returnedValue);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetOtherUserByUserName_InvalidUserName_ReturnsBadRequest()
    {
        // Arrange
        var validUserName = _testUser.UserName;

        // Act
        var response = await Client.GetOtherUserByUserName(validUserName.Substring(0, 4));
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<UserProfileDTO>(response.Content);

        // Assert
        Assert.Null(returnedValue);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestUser]
    public async Task UpdateUser_ValidData_ReturnsSuccessStatusCode()
    {
        // Arrange
        var userToUpdate = ExtractUpdateTestUserAttribute.UserForTest;
        userToUpdate.UserName = Guid.NewGuid().ToString();
        userToUpdate.Expertises.Add(new ExpertiseDTO
        {
            Id = _testExpertise.Id,
            Title = _testExpertise.Title,
        });

        // Act
        var response = await Client.Update(userToUpdate, TokenStorage.UserAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<UserDTO>(response.Content);

        // Assert
        Assert.NotNull(returnedValue);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestUser]
    public async Task UpdateUser_SameUserName_ReturnsBadRequest()
    {
        // Arrange
        var userToUpdate = ExtractUpdateTestUserAttribute.UserForTest;
        userToUpdate.UserName = _testUser.UserName;

        // Act
        var response = await Client.Update(userToUpdate, TokenStorage.UserAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<UserDTO>(response.Content);

        // Assert
        Assert.Null(returnedValue);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_ValidEmail_ReturnsSuccessStatusCode()
    {
        // Arrange
        await TokenStorage.GenerateNewTokens(_testUser);

        // Act
        var response = await Client.Delete(_testUser.Email!, TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestUser]
    public async Task DeleteUser_InvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var userToUpdate = ExtractUpdateTestUserAttribute.UserForTest;
        userToUpdate.Email = "invalid_email";

        // Act
        var response = await Client.Delete(_testUser.Email!, TokenStorage.UserAccessToken);
        bool.TryParse(response.Content, out bool result);

        // Assert
        Assert.False(result);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ForgotPassword_ValidEmail_ReturnsSuccessStatusCode()
    {
        // Arrange
        var forgotPassword = new ForgotPasswordDTO
        {
            Email = _testUser.Email!,
        };
        _factory.SetupMockEmailService();

        // Act
        var response = await Client.ForgotPassword(forgotPassword);

        // Assert
        _factory.EmailServiceMock.Verify(es => es.SendEmailAsync(It.IsAny<MessageData>()), Times.Once);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ForgotPassword_InvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var forgotPassword = new ForgotPasswordDTO
        {
            Email = "invalid_email",
        };

        // Act
        var response = await Client.ForgotPassword(forgotPassword);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateForgotPassword_ValidData_ReturnsSuccessStatusCode()
    {
        // Arrange
        var forgotPassword = new UpdateForgotPasswordDTO
        {
            Username = Uri.EscapeDataString(_testUser.UserName),
            Password = "Newpassword1!",
            ConfirmPassword = "Newpassword1!",
        };
        var token = await _userManager.GeneratePasswordResetTokenAsync(_testUser);
        forgotPassword.Token = Uri.EscapeDataString(token);

        // Act
        var response = await Client.UpdateForgotPassword(forgotPassword);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateForgotPassword_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var forgotPassword = new UpdateForgotPasswordDTO
        {
            Username = _testUser.UserName,
            Password = "newpassword1!",
            ConfirmPassword = "newpassword1!",
        };
        var token = await _userManager.GeneratePasswordResetTokenAsync(_testUser);
        forgotPassword.Token = token;

        // Act
        var response = await Client.UpdateForgotPassword(forgotPassword);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static string GenerateTestPassword()
    {
        string guid = Guid.NewGuid().ToString();
        return $"TestPass123_{guid.Substring(0, 10)}";
    }
}