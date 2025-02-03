using RestSharp;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Users.Expertises;

public class ExpertiseClient : BaseClient
{
    public ExpertiseClient(HttpClient client, string secondPartUrl = "")
        : base(client, secondPartUrl)
    {
    }

    public async Task<RestResponse> GetAll(string authToken = "")
    {
        return await SendQuery("/GetAll", authToken);
    }
}