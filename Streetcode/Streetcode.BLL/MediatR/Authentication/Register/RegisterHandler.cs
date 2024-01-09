using System.Text.RegularExpressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Authentication;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.SignUp
{
    public class RegisterHandler : IRequestHandler<RegisterQuery, Result<RegisterResponseDTO>>
    {
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;

        public RegisterHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger, IMapper mapper, UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<RegisterResponseDTO>> Handle(RegisterQuery request, CancellationToken cancellationToken)
        {
            User user = new User()
            {
                UserName = request.registerRequestDTO.UserName,
                Email = request.registerRequestDTO.Email,
                NormalizedEmail = request.registerRequestDTO.Email.ToUpper(),
                Name = request.registerRequestDTO.Name,
                Surname = request.registerRequestDTO.Surname,
                PhoneNumber = request.registerRequestDTO.PhoneNumber
            };
            string password = request.registerRequestDTO.Password;
            string passwordConfirmed = request.registerRequestDTO.PasswordConfirmed;

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
                    await _userManager.AddToRoleAsync(user, nameof(UserRole.User));
                }
                else
                {
                    _logger.LogError(request, result.Errors.FirstOrDefault()?.Description ?? "Error from UserManager while creating user");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }

            // Change it to mapper.
            var responseDTO = _mapper.Map<RegisterResponseDTO>(user);
            responseDTO.Password = password;
            responseDTO.Role = nameof(UserRole.User);

            return Result.Ok(responseDTO);
        }

        private async Task<Result> IsInputValid(User user, string password, string passwordConfirmed)
        {
            // Check if user valid.
            var userValidationResult = await IsUserValid(user);
            if (userValidationResult.IsFailed)
            {
                return Result.Fail(userValidationResult.Errors);
            }

            // Check if password and passwordConfirmed are same.
            if (password != passwordConfirmed)
            {
                return Result.Fail("cdcdscs");
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
            if (userFromDbDyUserName is not null)
            {
                return Result.Fail("User with such UserName already exists in database");
            }

            return Result.Ok();
        }
    }
}
