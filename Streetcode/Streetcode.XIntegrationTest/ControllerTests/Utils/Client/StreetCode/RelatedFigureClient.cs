using RestSharp;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;

public class RelatedFigureClient : StreetcodeRelatedBaseClient
{
    public RelatedFigureClient(
        HttpClient client,
        string secondPartUrl = "")
        : base(client, secondPartUrl)
    {
    }

    public async Task<RestResponse> GetByTagId(int id, string authToken = "")
    {
        return await SendQuery($"/GetByTagId/{id}", authToken);
    }

    public async Task<RestResponse> Create(int observerId, int targetId, string authToken = "")
    {
        var requestBody = new { observerId, targetId };
        return await SendCommand($"/Create/{observerId}&{targetId}", Method.Post, requestBody, authToken);
    }

    public async Task<RestResponse> Delete(int observerId, int targetId, string authToken = "")
    {
        var requestBody = new { observerId, targetId };
        return await SendCommand($"/Delete/{observerId}&{targetId}", Method.Delete, requestBody, authToken);
    }
}