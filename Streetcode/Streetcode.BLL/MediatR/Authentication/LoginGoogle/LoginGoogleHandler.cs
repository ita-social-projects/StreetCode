using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using FluentResults;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Util.Helpers;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Authentication.LoginGoogle;

public class LoginGoogleHandler : IRequestHandler<LoginGoogleQuery, Result<LoginResponseDto>>
{
    private const string LoginProvider = "Google";
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly ILoggerService _logger;
    private readonly UserManager<User> _userManager;

    public LoginGoogleHandler(
        IConfiguration configuration,
        IMapper mapper,
        ITokenService tokenService,
        ILoggerService logger,
        UserManager<User> userManager)
    {
        _configuration = configuration;
        _mapper = mapper;
        _tokenService = tokenService;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginGoogleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            });

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    Name = payload.GivenName,
                    Surname = payload.FamilyName
                };

                var uniqueUserName = UserHelper.EmailToUserNameConverter(user);
                user.UserName = uniqueUserName;

                var registerResponse = await RegisterUserAsync(request, user, LoginProvider, payload.Subject, LoginProvider);
                if (registerResponse.IsFailed)
                {
                    return Result.Fail(registerResponse.Errors);
                }
            }
            else
            {
                bool isUpdated = false;

                if (user.Name != payload.GivenName)
                {
                    user.Name = payload.GivenName;
                    isUpdated = true;
                }

                if (user.Surname != payload.FamilyName)
                {
                    user.Surname = payload.FamilyName;
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        string errorMessage =
                            updateResult.Errors.FirstOrDefault()?.Description ?? "Error updating user";
                        _logger.LogError(request, errorMessage);
                        return Result.Fail(errorMessage);
                    }
                }
            }

            string refreshToken = _tokenService.SetNewRefreshTokenForUser(user!);

            var jwtToken = await _tokenService.GenerateAccessTokenAsync(user);
            var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

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
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail($"Invalid Google Token: {ex.Message}");
        }
    }

    private async Task<Result> RegisterUserAsync(
        LoginGoogleQuery request,
        User user,
        string provider,
        string providerKey,
        string displayName)
    {
        try
        {
            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, nameof(UserRole.User));

                var loginInfo = new UserLoginInfo(provider, providerKey, displayName);
                var loginResult = await _userManager.AddLoginAsync(user, loginInfo);

                if (loginResult == IdentityResult.Failed())
                {
                    string errorMessage = loginResult.Errors.FirstOrDefault()?.Description ??
                                          "Error adding external login";
                    _logger.LogError(request, errorMessage);
                    return Result.Fail(errorMessage);
                }
            }
            else
            {
                string errorMessage = createResult.Errors.FirstOrDefault()?.Description ?? "Error creating user";
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