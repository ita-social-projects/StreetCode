using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent.Email.Messages;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.ForgotPassword;

public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Result<Unit>>
{
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<SendEmailHandler> _stringLocalizerEmailHandler;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly IStringLocalizer<UserSharedResource> _localizer;

    public ForgotPasswordHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, UserManager<User> userManager, IEmailService forgotPasswordEmailService, IStringLocalizer<UserSharedResource> localizer)
    {
        _logger = logger;
        _userManager = userManager;
        _emailService = forgotPasswordEmailService;
        _localizer = localizer;
    }

    public async Task<Result<Unit>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.ForgotPasswordDto.Email);
            if (user is null)
            {
                string errorMessage = _localizer["UserWithSuchUsernameNotExists"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var message = new ForgotPasswordMessage(
                new string[] { request.ForgotPasswordDto.Email },
                "",
                token,
                user.UserName);

            var isSuccess = await _emailService.SendEmailAsync(message);

            if (isSuccess)
            {
                return Result.Ok(Unit.Value);
            }

            string errorMsg = _stringLocalizerEmailHandler["FailedToSendEmailMessage"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}