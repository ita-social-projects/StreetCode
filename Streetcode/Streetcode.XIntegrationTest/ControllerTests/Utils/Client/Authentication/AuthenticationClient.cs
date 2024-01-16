using RestSharp;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.BLL.DTO.Authentication.Register;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Authentication
{
    public class AuthenticationClient : BaseClient
    {
        public AuthenticationClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> Login(LoginRequestDTO loginRequestDTO)
        {
            return await this.SendCommand("/Login", Method.Post, loginRequestDTO);
        }

        public async Task<RestResponse> Register(RegisterRequestDTO registerRequestDTO)
        {
            return await this.SendCommand("/Register", Method.Post, registerRequestDTO);
        }

        public async Task<RestResponse> RefreshToken(RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            return await this.SendCommand("/RefreshToken", Method.Post, refreshTokenRequestDTO);
        }
    }
}
