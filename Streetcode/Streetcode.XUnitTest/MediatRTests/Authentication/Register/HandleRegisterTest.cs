// <copyright file="HandleRegisterTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Authentication.Register;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.XUnitTest.MediatRTests.Authentication.Register
{
    using System.Linq.Expressions;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Query;
    using Moq;
    using Xunit;

    public class HandleRegisterTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IStringLocalizer<UserSharedResource>> _mockLocalizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleRegisterTest"/> class.
        /// </summary>
        public HandleRegisterTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();

            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mockLocalizer = new Mock<IStringLocalizer<UserSharedResource>>();

            _mockLocalizer.Setup(x => x["UserWithSuchEmailExists"]).Returns(new LocalizedString("UserWithSuchEmailExists", "UserWithSuchEmailExists"));
            _mockLocalizer.Setup(x => x["UserWithSuchUsernameExists"]).Returns(new LocalizedString("UserWithSuchUsernameExists", "UserWithSuchUsernameExists"));
            _mockLocalizer.Setup(x => x["UserManagerError"]).Returns(new LocalizedString("UserManagerError", "UserManagerError"));
        }

        [Fact]
        public async Task ShouldReturnSuccess_NewUser()
        {
            // Arrange.
            SetupServicesForSuccess();
            SetupMockMapper();
            var handler = GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(GetSampleRequestDto()), CancellationToken.None);

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
            var handler = GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(GetSampleRequestDto()), CancellationToken.None);

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
            var result = await handler.Handle(new RegisterQuery(GetSampleRequestDto()), CancellationToken.None);

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
            var result = await handler.Handle(new RegisterQuery(GetSampleRequestDto()), CancellationToken.None);

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
            var result = await handler.Handle(new RegisterQuery(GetSampleRequestDto()), CancellationToken.None);

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

        private static RegisterRequestDTO GetSampleRequestDto()
        {
            return new RegisterRequestDTO()
            {
                Email = "zero@gmail.com",
            };
        }

        private static RegisterRequestDTO GetRegisterRequestWithExistingEmail()
        {
            return new RegisterRequestDTO()
            {
                Email = "zero@gmail.com",
            };
        }

        private static RegisterRequestDTO GetRegisterRequestWithExistingUserName()
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
            _mockRepositoryWrapper
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

            _mockMapper
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

            _mockMapper
               .Setup(x => x
               .Map<User>(It.IsAny<RegisterRequestDTO>()))
               .Returns(sampleUser);
        }

        private void SetupMockUserManagerCreate(bool isSuccess)
        {
            _mockUserManager
                .Setup(manager => manager
                    .CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(isSuccess ? IdentityResult.Success : IdentityResult.Failed());
        }

        private void SetupMockUserManagerCreateThrowsException(string message)
        {
            _mockUserManager
                .Setup(manager => manager
                    .CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(message));
        }

        private void SetupMockUserManagerAddToRoleThrowsException(string message)
        {
            _mockUserManager
                .Setup(manager => manager
                    .AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(message));
        }

        private RegisterHandler GetRegisterHandler()
        {
            return new RegisterHandler(
                _mockRepositoryWrapper.Object,
                _mockLogger.Object,
                _mockMapper.Object,
                _mockUserManager.Object,
                _mockLocalizer.Object);
        }
    }
}
