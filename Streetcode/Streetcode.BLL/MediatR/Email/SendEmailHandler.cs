using System.Net.Http.Json;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.ReCaptchaResponseDTO;
using Streetcode.BLL.Factories.MessageDataFactory.Abstracts;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.BLL.MediatR.Email
{
    public class SendEmailHandler : IRequestHandler<SendEmailCommand, Result<Unit>>
    {
        private readonly IEmailService _emailService;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<SendEmailHandler> _stringLocalizer;
        private readonly IMessageDataAbstractFactory _messageDataAbstractFactory;
        private readonly ICaptchaService _captchaService;

        public SendEmailHandler(
            IEmailService emailService,
            ILoggerService logger,
            IStringLocalizer<SendEmailHandler> stringLocalizer,
            IMessageDataAbstractFactory messageDataAbstractFactory,
            ICaptchaService captchaService)
        {
            _emailService = emailService;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
            _messageDataAbstractFactory = messageDataAbstractFactory;
            _captchaService = captchaService;
        }

        public async Task<Result<Unit>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var reCaptchaValidationResult = await _captchaService.ValidateReCaptchaAsync(request.Email.Token, cancellationToken);
            if (reCaptchaValidationResult.IsFailed)
            {
                string captchaErrorMessage = reCaptchaValidationResult.Errors[0].Message;
                _logger.LogError(request, captchaErrorMessage);
                return Result.Fail(new Error(captchaErrorMessage));
            }

            var message = _messageDataAbstractFactory.CreateFeedbackMessageData(
                request.Email.From,
                request.Email.Source,
                request.Email.Content);

            bool isResultSuccess = await _emailService.SendEmailAsync(message);

            if (isResultSuccess)
            {
                return Result.Ok(Unit.Value);
            }

            string errorMsg = _stringLocalizer["FailedToSendEmailMessage"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
