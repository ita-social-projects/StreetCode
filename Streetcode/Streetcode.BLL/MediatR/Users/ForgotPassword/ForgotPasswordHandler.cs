using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Factories.MessageDataFactory.Abstracts;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Helpers;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMessageDataAbstractFactory _messageDataAbstractFactory;

    public ForgotPasswordHandler(
        ILoggerService logger,
        UserManager<User> userManager,
        IEmailService forgotPasswordEmailService,
        IStringLocalizer<UserSharedResource> localizer,
        IHttpContextAccessor httpContextAccessor,
        IMessageDataAbstractFactory messageDataAbstractFactory,
        IStringLocalizer<SendEmailHandler> stringLocalizerEmailHandler)
    {
        _logger = logger;
        _userManager = userManager;
        _emailService = forgotPasswordEmailService;
        _localizer = localizer;
        _httpContextAccessor = httpContextAccessor;
        _messageDataAbstractFactory = messageDataAbstractFactory;
        _stringLocalizerEmailHandler = stringLocalizerEmailHandler;
    }

    public async Task<Result<Unit>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.ForgotPasswordDto.Email);
            if (user is null)
            {
                string errorMessage = _localizer["UserWithSuchEmailNotFound"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var currentDomain = HttpContextHelper.GetCurrentDomain(_httpContextAccessor);

            var endcodedToken = Uri.EscapeDataString(token);
            var encodedUserName = Uri.EscapeDataString(user.UserName);

            var message = _messageDataAbstractFactory.CreateForgotPasswordMessageData(
                new string[] { request.ForgotPasswordDto.Email },
                endcodedToken,
                encodedUserName,
                currentDomain!);

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