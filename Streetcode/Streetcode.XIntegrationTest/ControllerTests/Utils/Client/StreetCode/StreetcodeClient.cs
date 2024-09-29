using RestSharp;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode
{
    public class StreetcodeClient : StreetcodeRelatedBaseClient
    {
        public StreetcodeClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetArtsByStreetcodeId(int id, string authToken = "")
        {
            return await this.SendQuery($"/getArtsByStreetcodeId/{id}", authToken);
        }

        public async Task<RestResponse> UpdateAsync(StreetcodeUpdateDTO updateStreetcodeDTO, string authToken = "")
        {
            return await this.SendCommand("/Update", Method.Put, updateStreetcodeDTO, authToken);
        }

        public async Task<RestResponse> CreateAsync(StreetcodeCreateDTO createStreetcodeDTO, string authToken = "")
        {
            return await this.SendCommand("/Create", Method.Post, createStreetcodeDTO, authToken);
        }
    }
}
