using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Factories.MessageDataFactory.Abstracts;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Helpers;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.Delete;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStringLocalizer<UserSharedResource> _localizer;
    private readonly IMessageDataAbstractFactory _messageDataAbstractFactory;
    private readonly IEmailService _emailService;

    public DeleteUserHandler(
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor,
        IStringLocalizer<UserSharedResource> localizer,
        IMessageDataAbstractFactory messageDataAbstractFactory,
        IEmailService emailService)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _localizer = localizer;
        _messageDataAbstractFactory = messageDataAbstractFactory;
        _emailService = emailService;
    }

    public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(HttpContextHelper.GetCurrentUserEmail(_httpContextAccessor));

            if (user is null)
            {
                string errorMessage = _localizer["UserWithSuchUsernameNotExists"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }

            if (user.Email != request.Email)
            {
                string errorMessage = _localizer["EmailNotMatch"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }

            _repositoryWrapper.UserRepository.Delete(user);
            await _repositoryWrapper.SaveChangesAsync();

            var messageData = _messageDataAbstractFactory.CreateDeleteConfirmationMessageData(new[] { user.Email });
            await _emailService.SendEmailAsync(messageData);

            return Result.Ok(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}