using RestSharp;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Additional
{
    public class CoordinateClient : BaseClient
    {
        public CoordinateClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetByStreetcodeId(int id, string authToken = "")
        {
            return await this.SendQuery($"/getByStreetcodeId/{id}", authToken);
        }
    }
}
