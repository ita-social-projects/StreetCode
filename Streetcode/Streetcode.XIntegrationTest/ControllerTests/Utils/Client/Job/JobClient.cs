using RestSharp;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Job
{
    public class JobClient : BaseClient
    {
        public JobClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> CreateAsync(JobCreateDto createJobDTO, string authToken = "")
        {
            return await this.SendCommand("/Create", Method.Post, createJobDTO, authToken);
        }

        public async Task<RestResponse> GetAllAsync(ushort? page, ushort? pageSize, string authToken = "")
        {
            return await this.SendQuery($"/GetAll?page={page}&pageSize={pageSize}", authToken);
        }

        public async Task<RestResponse> GetAllShortAsync(string authToken = "")
        {
            return await this.SendQuery("/GetAllShort", authToken);
        }

        public async Task<RestResponse> GetActiveJobsAsync(string authToken = "")
        {
            return await this.SendQuery("/GetActiveJobs", authToken);
        }

        public async Task<RestResponse> GetByIdAsync(int id, string authToken = "")
        {
            return await this.SendQuery($"/GetById/{id}", authToken);
        }

        public async Task<RestResponse> DeleteAsync(int id, string authToken = "")
        {
            return await this.SendCommand($"/Delete/{id}", RestSharp.Method.Delete, authToken);
        }

        public async Task<RestResponse> UpdateAsync(JobUpdateDto createJobDTO, string authToken = "")
        {
            return await this.SendCommand("/Update", Method.Put, createJobDTO, authToken);
        }

        public async Task<RestResponse> ChangeJobStatusAsync(JobChangeStatusDto jobChangeStatusDto,
            string authToken = "")
        {
            return await this.SendCommand("/ChangeJobStatus", RestSharp.Method.Put, jobChangeStatusDto, authToken);
        }
    }
}