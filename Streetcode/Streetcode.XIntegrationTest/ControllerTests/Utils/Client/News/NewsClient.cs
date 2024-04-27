using RestSharp;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.News
{
    public class NewsClient : BaseClient
    {
        public NewsClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetAllAsync(ushort page, ushort pageSize, string authToken = "")
        {
            return await this.SendQuery($"/GetAll?page={page}&pageSize={pageSize}", authToken);
        }
    }
}
