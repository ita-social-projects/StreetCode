using System.Net.Http.Json;
using FluentResults;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.DTO.ReCaptchaResponseDTO;
using Streetcode.BLL.Interfaces.Authentication;

namespace Streetcode.BLL.Services.Authentication
{
    public class CaptchaService : ICaptchaService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public CaptchaService(
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Result> ValidateReCaptchaAsync(string publicKey, CancellationToken? cancellationToken = null)
        {
            string reCaptchaUrl = _configuration["ReCaptcha:Url"] ?? string.Empty;
            string reCaptchaSecretKey = _configuration["ReCaptcha:SecretKey"] ?? string.Empty;
            string reCaptchaRequestUri = $"{reCaptchaUrl}?secret={reCaptchaSecretKey}&response={publicKey}";

            var reCaptchaResponse = cancellationToken is null ?
                await _httpClient.PostAsync(reCaptchaRequestUri, null) :
                await _httpClient.PostAsync(reCaptchaRequestUri, null, (CancellationToken)cancellationToken);

            if (!reCaptchaResponse.IsSuccessStatusCode)
            {
                string errorMessage = "ReCaptcha request failed";
                return Result.Fail(new Error(errorMessage));
            }

            var reCaptchaResponseResult = await reCaptchaResponse.Content
                .ReadFromJsonAsync<ReCaptchaResponseDto>(cancellationToken: cancellationToken ?? default);

            reCaptchaResponseResult ??= new();
            if (!reCaptchaResponseResult.Success)
            {
                string errorMessage = "Invalid captcha";
                return Result.Fail(new Error(errorMessage));
            }

            return Result.Ok();
        }
    }
}
