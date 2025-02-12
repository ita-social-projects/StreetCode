using RestSharp;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode.TextContent;

public class RelatedTermClient : StreetcodeRelatedBaseClient
{
    public RelatedTermClient(HttpClient client, string secondPathUrl = "")
        : base(client, secondPathUrl)
    {
    }

    public async Task<RestResponse> GetByTermId(int id, string authToken = "")
    {
        return await SendQuery($"/GetByTermId/{id}", authToken);
    }

    public async Task<RestResponse> Create(RelatedTermCreateDTO relatedTermCreateDto, string authToken = "")
    {
        return await SendCommand("/Create", Method.Post, relatedTermCreateDto, authToken);
    }

    public async Task<RestResponse> Update(int id, RelatedTermDTO relatedTermUpdateDto, string authToken = "")
    {
        return await SendCommand($"/Update/{id}", Method.Put, relatedTermUpdateDto, authToken);
    }

    public async Task<RestResponse> Delete(string word, string authToken = "")
    {
        return await SendCommand($"/Delete/{word}", Method.Delete, new RelatedTermDTO(), authToken);
    }
}
