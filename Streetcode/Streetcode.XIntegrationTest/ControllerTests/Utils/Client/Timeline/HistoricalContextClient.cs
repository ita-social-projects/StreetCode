using RestSharp;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Timeline;

public class HistoricalContextClient : StreetcodeRelatedBaseClient
{
    public HistoricalContextClient(HttpClient client, string secondPartUrl = "")
        : base(client, secondPartUrl)
    {
    }
    public async Task<RestResponse> UpdateAsync(HistoricalContextDTO updateTagDTO, string authToken = "")
    {
        return await this.SendCommand("/Update", Method.Put, updateTagDTO, authToken);
    }

    public async Task<RestResponse> CreateAsync(HistoricalContextDTO createTagDTO, string authToken = "")
    {
        return await this.SendCommand("/Create", Method.Post, createTagDTO, authToken);
    }

    public async Task<RestResponse> Delete(int id, string authToken = "")
    {
        return await this.SendCommand($"/Delete/{id}", Method.Delete, new HistoricalContextDTO(), authToken);
    }
}