using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Authentication.Register;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Authentication.Register
{
    public class HandleRegisterTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<UserManager<User>> _mockUserManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleRegisterTest"/> class.
        /// </summary>
        public HandleRegisterTest()
        {
            this._mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();

            var store = new Mock<IUserStore<User>>();
            this._mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task ShouldReturnSuccess_NewUser()
        {
            // Arrange.
            this.SetupServicesForSuccess();
            this.SetupMockMapper();
            var handler = this.GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(this.GetSampleRequestDTO()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldReturnRegisterResponseDTOWithCorrectData_NewUser()
        {
            // Arrange.
            string expectedId = Guid.NewGuid().ToString();
            string expectedRole = nameof(UserRole.User);
            this.SetupServicesForSuccess();
            this.SetupMockMapper(registerResponseDtoId: expectedId);
            var handler = this.GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(this.GetSampleRequestDTO()), CancellationToken.None);

            // Assert.
            Assert.Equal(expectedId, result.Value.Id);
            Assert.Equal(expectedRole, result.Value.Role);
        }

        //[Fact]
        //public async Task ShouldReturnFailWithCorrectMessage_UserWithGivenEmailIsAlreadyInDatabase()
        //{
        //    // Arrange.
        //    string expectedErrorMessage = "User with such Email already exists in database";
        //    this.SetupMockRepositoryGetFirstOrDefault(isExists: true);
        //    var handler = this.GetRegisterHandler();

        //    // Act.
        //    var result = await handler.Handle(new RegisterQuery(this.GetRegisterRequestWithExistingEmail()), CancellationToken.None);

        //    // Assert.
        //    Assert.True(result.IsFailed);
        //    Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault() !.Message);
        //}

        [Fact]
        public async Task ShouldReturnFailWithCorrectMessage_UserWithGivenUserNameIsAlreadyInDatabase()
        {
            // Arrange.
            string expectedErrorMessage = "User with such UserName already exists in database";
            this.SetupMockRepositoryGetFirstOrDefault(isExists: true);
            var handler = this.GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(this.GetRegisterRequestWithExistingUserName()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
            Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault() !.Message);
        }

        [Fact]
        public async Task ShouldReturnFailWithCorrectMessage_CreateUserFails()
        {
            // Arrange.
            string expectedErrorMessage = "Error from UserManager while creating user";
            this.SetupMockRepositoryGetFirstOrDefault(isExists: false);
            this.SetupMockUserManagerCreate(isSuccess: false);
            var handler = this.GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(this.GetSampleRequestDTO()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
            Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault() !.Message);
        }

        [Fact]
        public async Task ShouldReturnFailWithCorrectMessage_ExceptionIsThrownByCreateUser()
        {
            // Arrange.
            string expectedErrorMessage = "Exception is thrown from UserManager while creating user";
            this.SetupMockRepositoryGetFirstOrDefault(isExists: false);
            this.SetupMockUserManagerCreateThrowsException(expectedErrorMessage);
            var handler = this.GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(this.GetSampleRequestDTO()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
            Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault() !.Message);
        }

        [Fact]
        public async Task ShouldReturnFailWithCorrectMessage_ExceptionIsThrownByAddToRoleAsync()
        {
            // Arrange.
            string expectedErrorMessage = "Exception is thrown from UserManager while assigning role to user";
            this.SetupMockRepositoryGetFirstOrDefault(isExists: false);
            this.SetupMockUserManagerCreate(isSuccess: true);
            this.SetupMockUserManagerAddToRoleThrowsException(expectedErrorMessage);
            var handler = this.GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(this.GetSampleRequestDTO()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
            Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault() !.Message);
        }

        private User GetSampleUser()
        {
            return new User()
            {
                Email = "zero@gmail.com",
                UserName = "Zero_zero",
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
                UserName = string.Empty,
            };
        }

        private RegisterRequestDTO GetRegisterRequestWithExistingUserName()
        {
            return new RegisterRequestDTO()
            {
                Email = string.Empty,
                UserName = "Zero_zero",
            };
        }

        private void SetupServicesForSuccess()
        {
            this.SetupMockRepositoryGetFirstOrDefault(isExists: false);
            this.SetupMockUserManagerCreate(isSuccess: true);
        }

        private void SetupMockRepositoryGetFirstOrDefault(bool isExists)
        {
            this._mockRepositoryWrapper
                .Setup(wrapper => wrapper.UserRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(isExists ? this.GetSampleUser() : null);
        }

        private void SetupMockMapper(string registerResponseDtoId = "")
        {
            this._mockMapper
                .Setup(x => x
                .Map<RegisterResponseDTO>(It.IsAny<User>()))
                .Returns(new RegisterResponseDTO() { Id = registerResponseDtoId });
        }

        private void SetupMockUserManagerCreate(bool isSuccess)
        {
            this._mockUserManager
                .Setup(manager => manager
                    .CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(isSuccess ? IdentityResult.Success : IdentityResult.Failed());
        }

        private void SetupMockUserManagerCreateThrowsException(string message)
        {
            this._mockUserManager
                .Setup(manager => manager
                    .CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(message));
        }

        private void SetupMockUserManagerAddToRoleThrowsException(string message)
        {
            this._mockUserManager
                .Setup(manager => manager
                    .AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(message));
        }

        private RegisterHandler GetRegisterHandler()
        {
            return new RegisterHandler(
                this._mockRepositoryWrapper.Object,
                this._mockLogger.Object,
                this._mockMapper.Object,
                this._mockUserManager.Object);
        }
    }
}
