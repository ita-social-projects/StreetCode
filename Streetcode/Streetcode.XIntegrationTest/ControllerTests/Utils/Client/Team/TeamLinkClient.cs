using RestSharp;
using Streetcode.BLL.DTO.Team;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Team
{
    public class TeamLinkClient : BaseClient
    {
        public TeamLinkClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetAllAsync(string authToken = "")
        {
            return await this.SendQuery("/GetAll", authToken);
        }

        public async Task<RestResponse> CreateAsync(TeamMemberLinkCreateDTO teamMemberLink, string authToken = "")
        {
            return await this.SendCommand("/Create", Method.Post, teamMemberLink, authToken);
        }
    }
}
