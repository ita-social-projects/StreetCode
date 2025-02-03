using RestSharp;
using Streetcode.BLL.DTO.Team;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Additional;

public class TeamPositionsClient : StreetcodeRelatedBaseClient
{
    public TeamPositionsClient(HttpClient client, string secondPartUrl = "")
        : base(client, secondPartUrl)
    {
    }

    public async Task<RestResponse> GetByTitle(string title, string authToken = "")
    {
        return await this.SendQuery($"/GetByTitle/{title}", authToken);
    }

    public async Task<RestResponse> UpdateAsync(PositionDto positionDto, string authToken = "")
    {
        return await this.SendCommand("/Update", Method.Put, positionDto, authToken);
    }

    public async Task<RestResponse> CreateAsync(PositionDto positionDto, string authToken = "")
    {
        return await this.SendCommand("/Create", Method.Post, positionDto, authToken);
    }

    public async Task<RestResponse> Delete(int id, string authToken = "")
    {
        return await this.SendCommand($"/Delete/{id}", Method.Delete, new PositionDto(), authToken);
    }

    public async Task<RestResponse> GetAllWithTeamMembers(string authToken = "")
    {
        return await this.SendQuery($"/GetAllWithTeamMembers", authToken);
    }
}
