using RestSharp;
using Streetcode.BLL.DTO.Email;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Email;

public class EmailClient : BaseClient
{
    public EmailClient(HttpClient client, string secondPartUrl = "")
        : base(client, secondPartUrl)
    {
    }

    public async Task<RestResponse> Send(EmailDTO emailDto, string authToken = "")
    {
        return await SendCommand("/Send", Method.Post, emailDto, authToken);
    }
}