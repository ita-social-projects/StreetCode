using RestSharp;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Team
{
    public class TickerStringClient : BaseClient
    {
        public TickerStringClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetNameTickerString(string authToken = "")
        {
            return await this.SendQuery("/GetNameTickerString/", authToken);
        }
    }
}
