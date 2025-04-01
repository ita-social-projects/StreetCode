using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Factories.MessageDataFactory.Abstracts;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Helpers;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.MediatR.Authentication.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, Result<LoginResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILoggerService _logger;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<UserSharedResource> _localizer;
        private readonly ICaptchaService _captchaService;
        private readonly IEmailService _emailService;
        private readonly IMessageDataAbstractFactory _messageDataAbstractFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginHandler(
            IMapper mapper,
            ITokenService tokenService,
            ILoggerService logger,
            UserManager<User> userManager,
            ICaptchaService captchaService,
            IStringLocalizer<UserSharedResource> localizer,
            IEmailService emailService,
            IMessageDataAbstractFactory messageDataAbstractFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
            _userManager = userManager;
            _captchaService = captchaService;
            _localizer = localizer;
            _messageDataAbstractFactory = messageDataAbstractFactory;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<Result<LoginResponseDTO>> Handle(LoginQuery request, CancellationToken cancellationToken)
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
                if (user is null)
                {
                    var errorMessage = _localizer["UserWithSuchEmailNotFound"];
                    _logger.LogError(request, errorMessage);
                    return Result.Fail(errorMessage);
                }

                bool isPasswordValid = await _userManager.CheckPasswordAsync(user!, request.UserLogin.Password);
                if (!isPasswordValid)
                {
                    var errorMessage = _localizer["IncorrectPassword"];
                    _logger.LogError(request, errorMessage);
                    return Result.Fail(errorMessage);
                }

                var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!isEmailConfirmed)
                {
                    var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var currentDomain = HttpContextHelper.GetCurrentDomain(_httpContextAccessor) !;

                    var escapedToken = Uri.EscapeDataString(emailToken);
                    var escapedUsername = Uri.EscapeDataString(user.UserName);

                    var messageData = _messageDataAbstractFactory.CreateConfirmEmailMessageData(
                        new[] { user.Email! },
                        escapedToken,
                        escapedUsername,
                        currentDomain);

                    await _emailService.SendEmailAsync(messageData);

                    var errorMessage = _localizer["EmailNotConfirmed"];
                    _logger.LogError(request, errorMessage);
                    return Result.Fail(errorMessage);
                }

                string refreshToken = _tokenService.SetNewRefreshTokenForUser(user!);

                var token = await _tokenService.GenerateAccessTokenAsync(user!);
                var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

                var userDTO = _mapper.Map<UserDTO>(user);
                string? userRole = (await _userManager.GetRolesAsync(user !)).FirstOrDefault();
                userDTO.Role = userRole ?? "User";

                var response = new LoginResponseDTO()
                {
                    User = userDTO,
                    AccessToken = stringToken,
                    RefreshToken = refreshToken,
                };
                return Result.Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(request, e.Message);
                return Result.Fail(e.Message);
            }
        }
    }
}
