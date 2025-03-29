using RestSharp;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images
{
    public class StreetcodeArtSlideClient : BaseClient
    {
        public StreetcodeArtSlideClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetPageByStreetcodeId(int streetcodeId, int fromSlideN, int amountOfSlide, string authToken = "")
        {
            return await this.SendQuery($"/GetByStreetcodeId/{streetcodeId}?fromSlideN={fromSlideN}&amountOfSlides={amountOfSlide}", authToken);
        }
        public async Task<RestResponse> GetAllCountByStreetcodeId(uint id, string authToken = "")
        {
            return await this.SendQuery($"/GetByStreetcodeId/{id}", authToken);
        }

    }
}