using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Authentication.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, Result<LoginResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ITokenService _tokenService;
        private readonly ILoggerService _logger;
        private readonly UserManager<User> _userManager;

        public LoginHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService, ILoggerService logger, UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<LoginResponseDTO>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var userExistsExpression = GetUserExistsPredicate(request);
            var user = (await _repositoryWrapper.UserRepository
                .GetAllAsync())
                .FirstOrDefault(userExistsExpression);
            bool isUserNull = user is null;

            bool isValid = isUserNull ? false : await _userManager.CheckPasswordAsync(user, request.UserLogin.Password);
            if (isValid)
            {
                var token = _tokenService.GenerateJWTToken(user!);
                var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
                var userDTO = _mapper.Map<UserDTO>(user);
                userDTO.Password = request.UserLogin.Password;
                var response = new LoginResponseDTO()
                {
                    User = userDTO,
                    Token = stringToken,
                    ExpireAt = token.ValidTo
                };
                return Result.Ok(response);
            }

            string errorMessage = isUserNull ?
                "User with such Email and Username in not found" :
                "Password is incorrect";
            _logger.LogError(request, errorMessage);
            return Result.Fail(errorMessage);
        }

        private Func<User, bool> GetUserExistsPredicate(LoginQuery request)
        {
            Func<User, bool> userExistsPredicate = user =>
            {
                return user.UserName == request.UserLogin.Login || user.Email == request.UserLogin.Login;
            };
            return userExistsPredicate;
        }
    }
}
