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

        public async Task<RestResponse> Login(LoginRequestDto loginRequestDTO)
        {
            return await this.SendCommand("/Login", Method.Post, loginRequestDTO);
        }

        public async Task<RestResponse> Register(RegisterRequestDto registerRequestDTO)
        {
            return await this.SendCommand("/Register", Method.Post, registerRequestDTO);
        }

        public async Task<RestResponse> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDTO)
        {
            return await this.SendCommand("/RefreshToken", Method.Post, refreshTokenRequestDTO);
        }
    }
}
