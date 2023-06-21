using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.AdditionalContent.Email;

namespace Streetcode.BLL.MediatR.Email
{
    public class SendEmailHandler : IRequestHandler<SendEmailCommand, Result<Unit>>
    {
        private readonly IEmailService _emailService;
        private readonly ILoggerService? _logger;

        public SendEmailHandler(IEmailService emailService, ILoggerService? logger = null)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var message = new Message(new string[] { "streetcodeua@gmail.com" }, request.Email.From, "FeedBack", request.Email.Content);
            bool isResultSuccess = await _emailService.SendEmailAsync(message);

            if(isResultSuccess)
            {
                _logger?.LogInformation($"SendEmailCommand handled successfully");
                return Result.Ok(Unit.Value);
            }
            else
            {
                const string errorMsg = $"Failed to send email message";
                _logger?.LogError("SendEmailCommand handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
