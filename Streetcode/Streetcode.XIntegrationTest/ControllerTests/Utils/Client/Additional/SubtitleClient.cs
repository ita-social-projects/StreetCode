using RestSharp;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Additional
{
    public class SubtitleClient : BaseClient
    {
        public SubtitleClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetAllAsync(string authToken = "")
        {
            return await this.SendQuery($"/GetAll", authToken);
        }

        public async Task<RestResponse> GetByIdAsync(int id, string authToken = "")
        {
            return await this.SendQuery($"/GetById/{id}", authToken);
        }

        public async Task<RestResponse> GetByStreetcodeId(int id, string authToken = "")
        {
            return await this.SendQuery($"/getByStreetcodeId/{id}", authToken);
        }

        public async Task<RestResponse> GetTagByTitle(string title, string authToken = "")
        {
            return await this.SendQuery($"/GetTagByTitle/{title}", authToken);
        }
    }
}
