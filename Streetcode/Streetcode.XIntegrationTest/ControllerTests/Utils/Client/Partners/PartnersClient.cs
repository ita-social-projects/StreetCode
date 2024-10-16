using RestSharp;
using Streetcode.BLL.DTO.Partners;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Partners
{
    public class PartnersClient : StreetcodeRelatedBaseClient
    {
        public PartnersClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> CreateAsync(CreatePartnerDTO createPartnerDto, string authToken)
        {
            return await this.SendCommand("/Create", Method.Post, createPartnerDto, authToken);
        }
    }
}
