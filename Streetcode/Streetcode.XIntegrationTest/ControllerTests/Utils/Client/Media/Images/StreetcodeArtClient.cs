using RestSharp;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images
{
    public class StreetcodeArtClient : BaseClient
    {
        public StreetcodeArtClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetByStreetcodeId(int id, string authToken = "")
        {
            return await this.SendQuery($"/GetByStreetcodeId/{id}", authToken);
        }

        public async Task<RestResponse> GetPageByStreetcodeId(uint streetcodeId, ushort page, ushort pageSize, string authToken = "")
        {
            return await this.SendQuery($"/GetPageByStreetcodeId/{streetcodeId}?page={page}&pageSize={pageSize}", authToken);
        }
    }
}
