using RestSharp;
using RestSharp.Serializers;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client
{
    public abstract class BaseClient
    {
        protected RestClient client;

        public string SecondPartUrl { get; }

        public BaseClient(HttpClient client, string secondPartUrl = "")
        {
            this.client = new RestClient(client) { AcceptedContentTypes = ContentType.JsonAccept };
            this.SecondPartUrl = secondPartUrl;
        }

        public async Task<RestResponse> SendQuery(string requestString, string authToken = "")
        {
            var request = new RestRequest($"{this.SecondPartUrl}{requestString}");
            RestResponse response;
            try
            {
                response = await this.SendRequest(request, authToken);
            }
            catch (Exception ex)
            {
                // Add logging.
                return new RestResponse() { IsSuccessStatusCode = false, ErrorMessage = ex.Message };
            }

            return response;
        }

        public async Task<RestResponse> SendCommand<T>(string requestString, Method method, T requestDto, string authToken = "")
            where T : class
        {
            var request = new RestRequest($"{this.SecondPartUrl}{requestString}", method);
            request.AddJsonBody<T>(requestDto);
            RestResponse response;
            try
            {
                response = await this.SendRequest(request, authToken);
            }
            catch (Exception ex)
            {
                return new RestResponse() { IsSuccessStatusCode = false, ErrorMessage = ex.Message };
            }

            return response;
        }

        private async Task<RestResponse> SendRequest(RestRequest request, string authToken)
        {
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddHeader("authorization", $"Bearer {authToken}");
            request.AddHeader("content-type", "application/json");
            var response = await this.client.ExecuteAsync(request);
            return response;
        }
    }
}
