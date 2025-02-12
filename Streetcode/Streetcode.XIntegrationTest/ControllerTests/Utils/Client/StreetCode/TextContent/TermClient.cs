using RestSharp;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode.TextContent;

public class TermClient : StreetcodeRelatedBaseClient
{
    public TermClient(HttpClient client, string secondPathUrl = "")
        : base(client, secondPathUrl)
    {
    }

    public async Task<RestResponse> Create(TermCreateDTO streetcodeFactCreateDto, string authToken = "")
    {
        return await SendCommand("/Create", Method.Post, streetcodeFactCreateDto, authToken);
    }

    public async Task<RestResponse> Update(TermDTO termUpdateDto, string authToken = "")
    {
        return await SendCommand($"/Update", Method.Put, termUpdateDto, authToken);
    }

    public async Task<RestResponse> Delete(int id, string authToken = "")
    {
        return await SendCommand($"/Delete/{id}", Method.Delete, new TermDTO(), authToken);
    }
}
