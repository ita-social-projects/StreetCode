using RestSharp;
using RestSharp.Serializers;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base
{
    public class BaseClient
    {
        private readonly RestClient client;

        public BaseClient(HttpClient client, string secondPartUrl = "")
        {
            this.client = new RestClient(client) { AcceptedContentTypes = ContentType.JsonAccept };
            this.SecondPartUrl = secondPartUrl;
        }

        protected string SecondPartUrl { get; }

        protected async Task<RestResponse> SendQuery(string requestString, string authToken = "")
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

        protected async Task<RestResponse> SendCommand<T>(string requestString, Method method, T requestDto, string authToken = "")
            where T : class
        {
            var request = new RestRequest($"{this.SecondPartUrl}{requestString}", method);
            request.AddJsonBody(requestDto);
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
            request.AddHeader("Authorization", $"Bearer {authToken}");
            request.AddHeader("Content-Type", "application/json");

            var response = await this.client.ExecuteAsync(request);
            return response;
        }
    }
}
