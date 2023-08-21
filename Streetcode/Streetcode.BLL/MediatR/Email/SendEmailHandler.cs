using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.AdditionalContent.Email;

namespace Streetcode.BLL.MediatR.Email
{
    public class SendEmailHandler : IRequestHandler<SendEmailCommand, Result<Unit>>
    {
        private readonly IEmailService _emailService;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<SendEmailHandler> _stringLocalizer;

        public SendEmailHandler(IEmailService emailService, ILoggerService logger, IStringLocalizer<SendEmailHandler> stringLocalizer)
        {
            _emailService = emailService;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Result<Unit>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var message = new Message(new string[] { "streetcodeua@gmail.com" }, request.Email.From, "FeedBack", request.Email.Content);
            bool isResultSuccess = await _emailService.SendEmailAsync(message);

            if(isResultSuccess)
            {
                return Result.Ok(Unit.Value);
            }
            else
            {
                string errorMsg = _stringLocalizer["FailedToSendEmailMessage"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
