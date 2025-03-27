using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.MediatR.Authentication.ConfirmEmail;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager;
    private readonly IStringLocalizer<FailedToValidateSharedResource> _stringLocalizer;
    private readonly ILoggerService _logger;

    public ConfirmEmailHandler(
        UserManager<User> userManager,
        IStringLocalizer<FailedToValidateSharedResource> stringLocalizer,
        ILoggerService logger)
    {
        _userManager = userManager;
        _stringLocalizer = stringLocalizer;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var decodedUsername = Uri.UnescapeDataString(request.ConfirmEmailDto.UserName);
            var decodedToken = Uri.UnescapeDataString(request.ConfirmEmailDto.Token);

            var user = _userManager.Users.FirstOrDefault(u => u.UserName == decodedUsername);

            if (user is null || string.IsNullOrEmpty(decodedToken))
            {
                var errorMessage = _stringLocalizer["InvalidToken"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));

            return Result.OkIf(result.Succeeded, errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            throw;
        }
    }
}