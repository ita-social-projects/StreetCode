using RestSharp;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images
{
    public class ImageClient : StreetcodeRelatedBaseClient
    {
        public ImageClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetBaseImageAsync(int id)
        {
            return await this.SendQuery($"/getBaseImage/{id}");
        }

        public async Task<RestResponse> CreateAsync(ImageFileBaseCreateDTO image, string authToken = "")
        {
            return await this.SendCommand("/create", Method.Post, image, authToken);
        }

        public async Task<RestResponse> UpdateAsync(ImageFileBaseUpdateDTO image, string authToken = "")
        {
            return await this.SendCommand("/update", Method.Put, image, authToken);
        }
    }
}
