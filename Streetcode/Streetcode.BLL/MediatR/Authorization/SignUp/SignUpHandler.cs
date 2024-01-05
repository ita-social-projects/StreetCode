using System.Text.RegularExpressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.SignUp
{
    public class SignUpHandler : IRequestHandler<SignUpQuery, Result<UserDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public SignUpHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<UserDTO>> Handle(SignUpQuery request, CancellationToken cancellationToken)
        {
            User user = new User()
            {
                UserName = request.newUser.UserName,
                Email = request.newUser.Email,
                NormalizedEmail = request.newUser.Email.ToUpper(),
                Name = request.newUser.Name,
                Surname = request.newUser.Surname,
                PhoneNumber = request.newUser.PhoneNumber
            };
            string password = request.newUser.Password;
            string passwordConfirmed = request.newUser.PasswordConfirmed;

            // Validate input.
            var validationResult = await IsInputValid(user, password, passwordConfirmed);
            if (validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors);
            }

            try
            {
                await _repositoryWrapper.AuthRepository.RegisterAsync(user, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }

            // Change it to mapper.
            var userDTO = new UserDTO()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Id = user.Id,
                Login = user.UserName,
                Role = "user"
            };

            return Result.Ok(userDTO);
        }

        private async Task<Result> IsInputValid(User user, string password, string passwordConfirmed)
        {
            // Check if user valid.
            var userValidationResult = await IsUserValid(user);
            if (userValidationResult.IsFailed)
            {
                return Result.Fail(userValidationResult.Errors);
            }

            // Check if password valid.
            var passwordValidationResult = IsPasswordValid(password);
            if (!passwordValidationResult.isValid)
            {
                return Result.Fail(passwordValidationResult.errorMessage);
            }

            // Check if password and passwordConfirmed are same.
            if (password != passwordConfirmed)
            {
                return Result.Fail(passwordValidationResult.errorMessage);
            }

            return Result.Ok();
        }

        private async Task<Result> IsUserValid(User user)
        {
            // Check if user is unique by email.
            var userFromDbDyEmail = await _repositoryWrapper.UserRepository
                .GetFirstOrDefaultAsync(predicate: userFromDb => userFromDb.Email == user.Email);
            if (userFromDbDyEmail is not null)
            {
                return Result.Fail("User with such Email already exists in database");
            }

            // Check if user is unique by username.
            var userFromDbDyUserName = await _repositoryWrapper.UserRepository
                .GetFirstOrDefaultAsync(predicate: userFromDb => userFromDb.UserName == user.UserName);
            if (userFromDbDyEmail is not null)
            {
                return Result.Fail("User with such UserName already exists in database");
            }

            // Check if email valid.
            var emailValidationResult = IsEmailValid(user.Email);
            if (!emailValidationResult.isValid)
            {
                return Result.Fail(emailValidationResult.errorMassage);
            }

            return Result.Ok();
        }

        private (bool isValid, string? errorMassage) IsEmailValid(string email)
        {
            // if (Regex.IsMatch(email, @"[^a - zA - Z@.\d:]"))
            // {
            //    return (false, "Email cannot contain non-alphanumeric chracters(except @)");
            // }

            // if (email.ToCharArray().Count())
            // {
            //    return (false, "Email cannot contain non-alphanumeric chracters(except @)");
            // }

            // if (email.Contains("admin", StringComparison.CurrentCultureIgnoreCase))
            // {
            //    return (false, "Email cannot contain 'admin'");
            // }

            return (true, null);
        }

        private (bool isValid, string? errorMessage) IsPasswordValid(string password)
        {
            if (Regex.Matches(password, @"[\s]").Any())
            {
                return (false, "Password cannot contain whitespaces");
            }

            if (!Regex.Matches(password, @"[^a - zA - Z.\d:]").Any())
            {
                return (false, "Password must contain non-alphanumeric symbol");
            }

            if (password.Contains('%'))
            {
                return (false, "Password cannot contain '%'");
            }

            if (!Regex.Matches(password, @"\p{Lu}").Any())
            {
                return (false, "Password must contain UPPERCASE letter");
            }

            if (!Regex.Matches(password, @"\p{Ll}").Any())
            {
                return (false, "Password must contain lowercase letter");
            }

            return (true, null);
        }
    }
}
