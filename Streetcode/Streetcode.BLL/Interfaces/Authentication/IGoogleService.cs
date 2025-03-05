using Google.Apis.Auth;

namespace Streetcode.BLL.Interfaces.Authentication
{
    public interface IGoogleService
    {
        Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string id);
    }
}