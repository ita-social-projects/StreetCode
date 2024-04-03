using RestSharp;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images
{
    public class ArtClient : StreetcodeRelatedBaseClient
    {
        public ArtClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetArtsByStreetcodeId(int id, string authToken = "")
        {
            return await this.SendQuery($"/GetArtsByStreetcodeId/{id}", authToken);
        }
    }
}
