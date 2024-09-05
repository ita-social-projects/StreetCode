using RestSharp;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
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

        public async Task<RestResponse> UpdateAsync(UpdateTagDTO updateTagDTO, string authToken = "")
        {
            return await this.SendCommand("/Update", Method.Put, updateTagDTO, authToken);
        }

        public async Task<RestResponse> CreateAsync(CreateTagDTO createTagDTO, string authToken = "")
        {
            return await this.SendCommand("/Create", Method.Post, createTagDTO, authToken);
        }

        public async Task<RestResponse> Delete(int id, string authToken = "")
        {
            return await this.SendCommand($"/Delete/{id}", Method.Delete, new UpdateTagDTO(), authToken);
        }

    }
}
