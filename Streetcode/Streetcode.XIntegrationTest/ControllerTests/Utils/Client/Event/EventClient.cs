using RestSharp;
using Streetcode.BLL.DTO.Event.CreateUpdate;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Event
{
    public class EventClient : BaseClient
    {
        public EventClient(HttpClient client, string secondPartUrl = "/api/Events")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetAllAsync() => await this.SendQuery("/GetAll");

        public async Task<RestResponse> GetByIdAsync(int id) => await this.SendQuery($"/GetById/{id}");

        public async Task<RestResponse> CreateAsync(CreateUpdateEventDTO eventDto, string authToken = "")
            => await this.SendCommand($"/Create", Method.Post, eventDto, authToken);

        public async Task<RestResponse> DeleteAsync(int id, string authToken = "")
        {
            var dto = new { Id = id };
            return await this.SendCommand($"/Delete/{id}", Method.Delete, dto, authToken);
        }
    }
}
