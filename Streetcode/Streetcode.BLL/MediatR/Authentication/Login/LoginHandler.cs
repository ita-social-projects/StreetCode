using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.MediatR.Authentication.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, Result<LoginResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILoggerService _logger;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<UserSharedResource> _localizer;
        private readonly ICaptchaService _captchaService;

        public LoginHandler(
            IMapper mapper,
            ITokenService tokenService,
            ILoggerService logger,
            UserManager<User> userManager,
            ICaptchaService captchaService,
            IStringLocalizer<UserSharedResource> localizer)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
            _userManager = userManager;
            _captchaService = captchaService;
            _localizer = localizer;
        }

        public async Task<Result<LoginResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var reCaptchaValidationResult = await _captchaService.ValidateReCaptchaAsync(request.UserLogin.CaptchaToken, cancellationToken);
                if (reCaptchaValidationResult.IsFailed)
                {
                    string captchaErrorMessage = reCaptchaValidationResult.Errors[0].Message;
                    _logger.LogError(request, captchaErrorMessage);
                    return Result.Fail(new Error(captchaErrorMessage));
                }

                var user = await _userManager.FindByEmailAsync(request.UserLogin.Login);
                bool isUserNull = user is null;

                bool isValid = isUserNull ? false : await _userManager.CheckPasswordAsync(user!, request.UserLogin.Password);
                if (isValid)
                {
                    string refreshToken = _tokenService.SetNewRefreshTokenForUser(user!);

                    var token = await _tokenService.GenerateAccessTokenAsync(user!);
                    var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

                    var userDTO = _mapper.Map<UserDto>(user);
                    string? userRole = (await _userManager.GetRolesAsync(user !)).FirstOrDefault();
                    userDTO.Role = userRole ?? "User";

                    var response = new LoginResponseDto()
                    {
                        User = userDTO,
                        AccessToken = stringToken,
                        RefreshToken = refreshToken,
                    };
                    return Result.Ok(response);
                }

                string errorMessage = isUserNull ?
                    _localizer["UserWithSuchEmailNotFound"] :
                    _localizer["IncorrectPassword"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(request, e.Message);
                return Result.Fail(e.Message);
            }
        }
    }
}
