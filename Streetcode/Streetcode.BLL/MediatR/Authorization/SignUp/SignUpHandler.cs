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
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;

        public SignUpHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger, IMapper mapper, UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
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
                // Create user with given password and assign 'user' role to it.
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }

            // Change it to mapper.
            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.Password = password;

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
            // Check if email valid.
            var emailValidationResult = IsEmailValid(user.Email);
            if (!emailValidationResult.isValid)
            {
                return Result.Fail(emailValidationResult.errorMassage);
            }

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
            if (userFromDbDyUserName is not null)
            {
                return Result.Fail("User with such UserName already exists in database");
            }

            return Result.Ok();
        }

        private (bool isValid, string? errorMassage) IsEmailValid(string email)
        {
            // Check if input email has standart format( e.g. *******@****.com).
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|ua)$"))
            {
                return (false, "Incorrect email address format");
            }

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
