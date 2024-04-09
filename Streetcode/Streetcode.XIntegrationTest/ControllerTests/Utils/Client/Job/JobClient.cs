using RestSharp;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Job
{
    public class JobClient: BaseClient
    {
        public JobClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> CreateAsync(JobDto createJobDTO, string authToken = "")
        {
            return await this.SendCommand("/Create", Method.Post, createJobDTO, authToken);
        }
    }
}
