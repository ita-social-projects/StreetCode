using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.MediatR.Authentication.ValidateToken;

public class ValidateTokenHandler : IRequestHandler<ValidateTokenCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<UserSharedResource> _localizerUserShared;
    private readonly IStringLocalizer<FailedToValidateSharedResource> _localizerFailedToValidate;

    public ValidateTokenHandler(
        UserManager<User> userManager,
        ILoggerService logger,
        IStringLocalizer<UserSharedResource> localizerUserShared,
        IStringLocalizer<FailedToValidateSharedResource> localizerFailedToValidate)
    {
        _userManager = userManager;
        _logger = logger;
        _localizerUserShared = localizerUserShared;
        _localizerFailedToValidate = localizerFailedToValidate;
    }

    public async Task<Result<bool>> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.UserName == request.ValidateTokenDto.UserName);
        if (user is null)
        {
            var errorMessage = _localizerUserShared["UserNotFound"];
            _logger.LogError(request, errorMessage);
            return Result.Fail(errorMessage);
        }

        try
        {
            bool result;
            if (request.ValidateTokenDto.TokenProvider == "Default")
            {
                result = await _userManager.VerifyUserTokenAsync(
                    user,
                    TokenOptions.DefaultProvider,
                    request.ValidateTokenDto.Purpose,
                    request.ValidateTokenDto.Token);
            }
            else
            {
                result = await _userManager.VerifyUserTokenAsync(
                    user,
                    request.ValidateTokenDto.TokenProvider,
                    request.ValidateTokenDto.Purpose,
                    request.ValidateTokenDto.Token);
            }

            return Result.OkIf(result, _localizerFailedToValidate["InvalidToken"]);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}