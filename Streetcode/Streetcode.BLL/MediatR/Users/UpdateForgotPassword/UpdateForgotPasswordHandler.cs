using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Users.ForgotPassword;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.UpdateForgotPassword;

public class UpdateForgotPasswordHandler : IRequestHandler<UpdateForgotPasswordCommand, Result<Unit>>
{
    private readonly ILoggerService _logger;
    private readonly UserManager<User> _userManager;
    private readonly IStringLocalizer<UserSharedResource> _localizer;

    public UpdateForgotPasswordHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, UserManager<User> userManager, IStringLocalizer<UserSharedResource> localizer)
    {
        _logger = logger;
        _userManager = userManager;
        _localizer = localizer;
    }

    public async Task<Result<Unit>> Handle(UpdateForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.UpdateForgotPasswordDto.Token) || string.IsNullOrEmpty(request.UpdateForgotPasswordDto.Username))
            {
                string errorMessage = _localizer["UserWithSuchUsernameNotExists"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }

            var decodedUserName = Uri.UnescapeDataString(request.UpdateForgotPasswordDto.Username);
            var decodedToken = Uri.UnescapeDataString(request.UpdateForgotPasswordDto.Token);

            var user = await _userManager.FindByNameAsync(decodedUserName);

            if (user is null)
            {
                string errorMessage = _localizer["UserWithSuchUsernameNotExists"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                decodedToken,
                request.UpdateForgotPasswordDto.Password);

            if (result.Succeeded)
            {
                return Result.Ok(Unit.Value);
            }

            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError(request, string.Join(" ", errorMessages));
            return Result.Fail(errorMessages);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}