using System.Net.Http.Json;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.ReCaptchaResponseDTO;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.AdditionalContent.Email;
using Streetcode.DAL.Entities.AdditionalContent.Email.Messages;

namespace Streetcode.BLL.MediatR.Email
{
    public class SendEmailHandler : IRequestHandler<SendEmailCommand, Result<Unit>>
    {
        private readonly IEmailService _emailService;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<SendEmailHandler> _stringLocalizer;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SendEmailHandler(IEmailService emailService, ILoggerService logger, IStringLocalizer<SendEmailHandler> stringLocalizer, HttpClient httpClient, IConfiguration configuration)
        {
            _emailService = emailService;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Result<Unit>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var reCaptchaResponse = await _httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_configuration["ReCaptcha:SecretKey"]}&response={request.Email.Token}", null, cancellationToken);

            if (!reCaptchaResponse.IsSuccessStatusCode)
            {
                string errorMessage = _stringLocalizer["RecaptchaRequestFailed"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(new Error(errorMessage));
            }

            var reCaptchaResponseResult = await reCaptchaResponse.Content.ReadFromJsonAsync<ReCaptchaResponseDto>(cancellationToken: cancellationToken);

            reCaptchaResponseResult ??= new();
            if (!reCaptchaResponseResult.Success)
            {
                string errorMessage = _stringLocalizer["InvalidCaptcha"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(new Error(errorMessage));
            }

            var message = new FeedbackMessage(
                new string[] { _configuration["EmailConfiguration:To"] ?? "stagestreetcodedev@gmail.com" },
                request.Email.From,
                request.Email.Source,
                "FeedBack",
                request.Email.Content);
            bool isResultSuccess = await _emailService.SendEmailAsync(message);

            if (isResultSuccess)
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
