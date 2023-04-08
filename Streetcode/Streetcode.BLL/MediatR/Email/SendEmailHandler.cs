using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.DAL.Entities.AdditionalContent.Email;

namespace Streetcode.BLL.MediatR.Email
{
    public class SendEmailHandler : IRequestHandler<SendEmailCommand, Result<Unit>>
    {
        private readonly IEmailService _emailService;

        public SendEmailHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<Result<Unit>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var message = new Message(new string[] { "streetcodeua@gmail.com" }, request.Email.From, "FeedBack", request.Email.Content);
            bool isResultSuccess = await _emailService.SendEmailAsync(message);

            return isResultSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to send email message"));
        }
    }
}
