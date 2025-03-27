using RestSharp;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode.TextContent;

public class FactClient : StreetcodeRelatedBaseClient
{
    public FactClient(HttpClient client, string secondPathUrl = "")
        : base(client, secondPathUrl)
    {
    }

    public async Task<RestResponse> Create(StreetcodeFactCreateDTO streetcodeFactCreateDto, string authToken = "")
    {
        return await SendCommand("/Create", Method.Post, streetcodeFactCreateDto, authToken);
    }

    public async Task<RestResponse> Update(int id, StreetcodeFactUpdateDTO factUpdateDto, string authToken = "")
    {
        return await SendCommand($"/Update/{id}", Method.Put, factUpdateDto, authToken);
    }

    public async Task<RestResponse> Delete(int id, string authToken = "")
    {
        return await SendCommand($"/Delete/{id}", Method.Delete, new FactDto(), authToken);
    }
}
