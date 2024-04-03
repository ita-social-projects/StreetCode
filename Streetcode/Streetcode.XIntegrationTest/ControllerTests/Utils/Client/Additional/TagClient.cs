using RestSharp;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Tag
{
    public class TagClient: StreetcodeRelatedBaseClient
    {
        public TagClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetTagByTitle(string title, string authToken = "")
        {
            return await this.SendQuery($"/GetTagByTitle/{title}", authToken);
        }
    }
}
