using System.Text.RegularExpressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Authentication.Register;
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

            // Validate input.
            var uniqueResult = await IsUserUnique(user);
            if (uniqueResult.IsFailed)
            {
                return Result.Fail(uniqueResult.Errors);
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
                    string errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Error from UserManager while creating user";
                    _logger.LogError(request, errorMessage);
                    return Result.Fail(errorMessage);
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

        private async Task<Result> IsUserUnique(User user)
        {
            // Check if user is unique by email.
            var userFromDbDyEmail = await _repositoryWrapper.UserRepository
                .GetFirstOrDefaultAsync(predicate: userFromDb => userFromDb.Email == user.Email || userFromDb.UserName == user.UserName);
            if (userFromDbDyEmail is not null)
            {
                bool isNotUniqueByEmail = userFromDbDyEmail.Email == user.Email;
                return Result.Fail($"User with such {(isNotUniqueByEmail ? "Email" : "UserName")} already exists in database");
            }

            return Result.Ok();
        }
    }
}
