using RestSharp;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

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
            return await SendCommand("/Login", Method.Post, loginRequestDTO);
        }

        public async Task<RestResponse> Register(RegisterRequestDTO registerRequestDTO)
        {
            return await SendCommand("/Register", Method.Post, registerRequestDTO);
        }

        public async Task<RestResponse> RefreshToken(RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            return await SendCommand("/RefreshToken", Method.Post, refreshTokenRequestDTO);
        }

        public async Task<RestResponse> Logout(string authToken = "")
        {
            return await SendCommand("/Logout", Method.Post, authToken: authToken);
        }
    }
}
