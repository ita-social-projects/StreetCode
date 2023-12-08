namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    using System.Threading.Tasks;
    using global::Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
    using RestSharp;
    using RestSharp.Serializers;

    public class StreetcodeClient
    {
        protected RestClient Client;

        public string SecondPartUrl { get; }

      public StreetcodeClient(HttpClient client, string secondPartUrl = "")
        {
            this.Client = new RestClient(client) { AcceptedContentTypes=ContentType.JsonAccept };
            this.SecondPartUrl = secondPartUrl;
        }

        public async Task<RestResponse> GetAllAsync()
        {
            return await this.GetResponse($"/GetAll");
        }

        public async Task<RestResponse> GetByIdAsync(int id)
        {
            return await this.GetResponse($"/GetById/{id}");
        }

        public async Task<RestResponse> GetByStreetcodeId(int id)
        {
            return await this.GetResponse($"/getByStreetcodeId/{id}");
        }

        public async Task<RestResponse> GetArtsByStreetcodeId(int id)
        {
            return await this.GetResponse($"/getArtsByStreetcodeId/{id}");
        }

        public async Task<RestResponse> GetResponse(string requestString)
        {
            var request = new RestRequest($"{this.SecondPartUrl}{requestString}");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddHeader("content-type", "application/json");
            var returns = await this.Client.ExecuteGetAsync(request);
            return returns;
        }

        public async Task<RestResponse> UpdateAsync(UpdateStreetcodeCommand updateStreetcodeCommand)
        {
            var request = new RestRequest($"{this.SecondPartUrl}/Update", Method.Put);
            request.AddJsonBody(updateStreetcodeCommand);
            var response = await this.Client.ExecuteAsync(request);
            return response;
        }
    }
}
