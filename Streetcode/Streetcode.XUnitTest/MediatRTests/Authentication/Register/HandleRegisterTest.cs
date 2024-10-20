// <copyright file="HandleRegisterTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Authentication.Register;
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
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepositoryWrapper> mockRepositoryWrapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<UserManager<User>> mockUserManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleRegisterTest"/> class.
        /// </summary>
        public HandleRegisterTest()
        {
            this.mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();

            var store = new Mock<IUserStore<User>>();
            this.mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
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
            User expectedUser = this.GetSampleUser();
            string expectedRole = nameof(UserRole.User);
            this.SetupServicesForSuccess();
            this.SetupMockMapper(expectedUser);
            var handler = this.GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(this.GetSampleRequestDTO()), CancellationToken.None);

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
            string expectedErrorMessage = "User with such Email already exists in database";
            this.SetupMockRepositoryGetFirstOrDefault(isExists: true);
            this.SetupMockMapper(isEmailExists: true);
            var handler = this.GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(this.GetRegisterRequestWithExistingEmail()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldReturnFailWithCorrectMessage_UserWithGivenUserNameIsAlreadyInDatabase()
        {
            // Arrange.
            string expectedErrorMessage = "User with such UserName already exists in database";
            this.SetupMockRepositoryGetFirstOrDefault(isExists: true);
            this.SetupMockMapper();
            var handler = this.GetRegisterHandler();

            // Act.
            var result = await handler.Handle(new RegisterQuery(this.GetRegisterRequestWithExistingUserName()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
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
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
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
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
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
            this.mockRepositoryWrapper
                .Setup(wrapper => wrapper.UserRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Func<IQueryable<User>,
                        IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(isExists ? this.GetSampleUser() : null);
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

            this.mockMapper
                .Setup(x => x
                .Map<RegisterResponseDTO>(It.IsAny<User>()))
                .Returns(registerResponseToReturnFromMapper);

            User sampleUser = this.GetSampleUser();
            if (isEmailExists)
            {
                sampleUser.UserName = string.Empty;
            }
            else
            {
                sampleUser.Email = string.Empty;
            }

            this.mockMapper
               .Setup(x => x
               .Map<User>(It.IsAny<RegisterRequestDTO>()))
               .Returns(sampleUser);
        }

        private void SetupMockUserManagerCreate(bool isSuccess)
        {
            this.mockUserManager
                .Setup(manager => manager
                    .CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(isSuccess ? IdentityResult.Success : IdentityResult.Failed());
        }

        private void SetupMockUserManagerCreateThrowsException(string message)
        {
            this.mockUserManager
                .Setup(manager => manager
                    .CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(message));
        }

        private void SetupMockUserManagerAddToRoleThrowsException(string message)
        {
            this.mockUserManager
                .Setup(manager => manager
                    .AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(message));
        }

        private RegisterHandler GetRegisterHandler()
        {
            return new RegisterHandler(
                this.mockRepositoryWrapper.Object,
                this.mockLogger.Object,
                this.mockMapper.Object,
                this.mockUserManager.Object);
        }
    }
}
