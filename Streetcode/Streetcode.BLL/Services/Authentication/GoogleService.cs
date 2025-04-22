using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.Interfaces.Authentication;

namespace Streetcode.BLL.Services.Authentication
{
    public class GoogleService : IGoogleService
    {
        private readonly IConfiguration _configuration;
        public GoogleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string id)
        {
            if (string.IsNullOrEmpty(_configuration["Authentication:Google:ClientId"]))
            {
                throw new InvalidOperationException("Google ClientId configuration is missing");
            }

            var payload = await GoogleJsonWebSignature.ValidateAsync(id, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            });

            return payload;
        }
    }
}