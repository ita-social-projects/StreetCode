using RestSharp;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode.TextContent;

public class TermClient : StreetcodeRelatedBaseClient
{
    public TermClient(HttpClient client, string secondPathUrl = "")
        : base(client, secondPathUrl)
    {
    }

    public async Task<RestResponse> Create(TermCreateDto streetcodeFactCreateDto, string authToken = "")
    {
        return await SendCommand("/Create", Method.Post, streetcodeFactCreateDto, authToken);
    }

    public async Task<RestResponse> Update(TermDto termUpdateDto, string authToken = "")
    {
        return await SendCommand($"/Update", Method.Put, termUpdateDto, authToken);
    }

    public async Task<RestResponse> Delete(int id, string authToken = "")
    {
        return await SendCommand($"/Delete/{id}", Method.Delete, new TermDto(), authToken);
    }
}
