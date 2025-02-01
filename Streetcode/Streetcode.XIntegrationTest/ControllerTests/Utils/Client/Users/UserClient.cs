using RestSharp;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.DTO.Users.Password;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Users;

public class UserClient : BaseClient
{
    public UserClient(HttpClient client, string secondPartUrl = "")
        : base(client, secondPartUrl)
    {
    }

    public async Task<RestResponse> GetByUserName(string authToken = "")
    {
        return await SendQuery("/GetByUserName", authToken);
    }

    public async Task<RestResponse> ExistWithUserName(string userName, string authToken = "")
    {
        return await SendQuery($"/ExistWithUserName/{userName}", authToken);
    }

    public async Task<RestResponse> GetOtherUserByUserName(string otherUserName, string authToken = "")
    {
        return await SendQuery($"/GetOtherUserByUserName/{otherUserName}", authToken);
    }

    public async Task<RestResponse> Update(UpdateUserDTO updateUserDto, string authToken = "")
    {
        return await SendCommand("/Update", Method.Put, updateUserDto, authToken);
    }

    public async Task<RestResponse> Delete(string email, string authToken = "")
    {
        return await SendCommand($"/Delete/{email}", Method.Delete, authToken);
    }

    public async Task<RestResponse> ForgotPassword(ForgotPasswordDTO updateForgotPasswordDto, string authToken = "")
    {
        return await SendCommand("/ForgotPassword", Method.Post, updateForgotPasswordDto, authToken);
    }

    public async Task<RestResponse> UpdateForgotPassword(UpdateForgotPasswordDTO updateForgotPasswordDto, string authToken = "")
    {
        return await SendCommand("/UpdateForgotPassword", Method.Put, updateForgotPasswordDto, authToken);
    }
}