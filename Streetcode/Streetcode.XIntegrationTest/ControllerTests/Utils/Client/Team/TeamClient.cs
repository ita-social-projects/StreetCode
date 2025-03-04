using RestSharp;
using Streetcode.BLL.DTO.Team;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Team
{
    public class TeamClient : BaseClient
    {
        public TeamClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetAllAsync(ushort page, ushort pageSize, string authToken = "")
        {
            return await this.SendQuery($"/GetAll?page={page}&pageSize={pageSize}", authToken);
        }

        public async Task<RestResponse> GetAllMainAsync(string authToken = "")
        {
            return await this.SendQuery($"/GetAllMain", authToken);
        }

        public async Task<RestResponse> GetByIdAsync(int id, string authToken = "")
        {
            return await this.SendQuery($"/GetById/{id}", authToken);
        }

        public async Task<RestResponse> GetByRoleIdAsync(int id, string authToken = "")
        {
            return await this.SendQuery($"/GetByRoleId/{id}", authToken);
        }

        public async Task<RestResponse> CreateAsync(TeamMemberCreateDTO createTeamMemberDTO, string authToken = "")
        {
            return await this.SendCommand("/Create", Method.Post, createTeamMemberDTO, authToken);
        }

        public async Task<RestResponse> UpdateAsync(UpdateTeamMemberDTO createTeamMemberDTO, string authToken = "")
        {
            return await this.SendCommand("/Update", Method.Put, createTeamMemberDTO, authToken);
        }

        public async Task<RestResponse> DeleteAsync(int id, string authToken = "")
        {
            return await this.SendCommand($"/Delete/{id}", Method.Delete, new TeamMemberDTO(), authToken);
        }
    }
}
