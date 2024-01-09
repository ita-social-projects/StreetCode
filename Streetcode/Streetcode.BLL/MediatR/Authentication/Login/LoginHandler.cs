using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Authentication;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, Result<LoginResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ITokenService _tokenService;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<LoginHandler> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService, ILoggerService logger, IStringLocalizer<LoginHandler> stringLocalizer, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<LoginResponseDTO>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _repositoryWrapper.UserRepository
               .GetFirstOrDefaultAsync(user => user.UserName == request.UserLogin.Login || user.Email == request.UserLogin.Login);

            bool isValid = user is null ? false : await _userManager.CheckPasswordAsync(user, request.UserLogin.Password);
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

            string errorMsg = _stringLocalizer["UserNotFound"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail("error");
        }
    }
}
