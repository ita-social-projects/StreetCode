using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Authentication.Register
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
            User user = _mapper.Map<User>(request.registerRequestDTO);
            string password = request.registerRequestDTO.Password;

            var uniqueResult = await IsUserUnique(user);
            if (uniqueResult.IsFailed)
            {
                return Result.Fail(uniqueResult.Errors);
            }

            var registerResponse = await RegisterUserAsync(request, user, password);
            if (registerResponse.IsFailed)
            {
                return Result.Fail(registerResponse.Errors);
            }

            var responseDTO = _mapper.Map<RegisterResponseDTO>(user);
            responseDTO.Password = password;
            responseDTO.Role = nameof(UserRole.User);

            return Result.Ok(responseDTO);
        }

        private async Task<Result> IsUserUnique(User user)
        {
            // Check if user is unique by email or username.
            var userFromDbDyEmail = await _repositoryWrapper.UserRepository
                .GetFirstOrDefaultAsync(predicate: userFromDb => userFromDb.Email == user.Email || userFromDb.UserName == user.UserName);
            if (userFromDbDyEmail is not null)
            {
                bool isNotUniqueByEmail = userFromDbDyEmail.Email == user.Email;
                return Result.Fail($"User with such {(isNotUniqueByEmail ? "Email" : "UserName")} already exists in database");
            }

            return Result.Ok();
        }

        private async Task<Result> RegisterUserAsync(RegisterQuery request, User user, string password)
        {
            try
            {
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

            return Result.Ok();
        }
    }
}
