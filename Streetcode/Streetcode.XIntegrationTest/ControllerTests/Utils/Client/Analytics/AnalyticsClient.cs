using RestSharp;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Analytics;

public class AnalyticsClient : BaseClient
{
    public AnalyticsClient(HttpClient client, string secondPartUrl = "")
        : base(client, secondPartUrl)
    {
    }

    public async Task<RestResponse> Update(int id, string authToken = "")
    {
        return await SendCommand($"/Update/{id}", Method.Put, authToken);
    }

    public async Task<RestResponse> GetAll(string authToken = "")
    {
        return await SendQuery($"/GetAll", authToken);
    }

    public async Task<RestResponse> GetByQrId(int qrId, string authToken = "")
    {
        return await SendQuery($"/GetByQrId/{qrId}", authToken);
    }

    public async Task<RestResponse> ExistByQrId(int qrId, string authToken = "")
    {
        return await SendQuery($"/ExistByQrId/{qrId}", authToken);
    }

    public async Task<RestResponse> GetAllByStreetcodeId(int streetcodeId, string authToken = "")
    {
        return await SendQuery($"/GetAllByStreetcodeId/{streetcodeId}", authToken);
    }

    public async Task<RestResponse> Delete(int id, string authToken = "")
    {
        return await SendCommand($"/Delete/{id}", Method.Delete, authToken);
    }
}