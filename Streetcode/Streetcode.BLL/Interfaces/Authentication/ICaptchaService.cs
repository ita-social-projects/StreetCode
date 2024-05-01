using FluentResults;

namespace Streetcode.BLL.Interfaces.Authentication
{
    public interface ICaptchaService
    {
        public Task<Result> ValidateReCaptchaAsync(string captchaToken, CancellationToken? cancellationToken);
    }
}
