namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    using RestSharp;
    using RestSharp.Serializers;
    using System.Threading.Tasks;

    public class StreetcodeClient
    {
        protected RestClient Client;

        public string SecondPartUrl { get; }

        public StreetcodeClient(HttpClient client,string secondPartUrl = "")
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

        public async Task<RestResponse> GetResponse(string requestString)
        {
            var request = new RestRequest($"{this.SecondPartUrl}{requestString}");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddHeader("content-type", "application/json");
            
            return await this.Client.ExecuteGetAsync(request);
        }

    }
}
