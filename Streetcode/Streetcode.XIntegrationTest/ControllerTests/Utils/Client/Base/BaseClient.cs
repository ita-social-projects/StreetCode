using RestSharp;
using RestSharp.Serializers;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base
{
    public class BaseClient
    {
        private readonly RestClient _client;

        protected readonly string _secondPartUrl;

        public BaseClient(HttpClient client, string secondPartUrl = "")
        {
            _client = new RestClient(client) { AcceptedContentTypes = ContentType.JsonAccept };
            _secondPartUrl = secondPartUrl;
        }

        protected async Task<RestResponse> SendQuery(string requestString, string authToken = "")
        {
            var request = new RestRequest($"{_secondPartUrl}{requestString}");
            RestResponse response;
            try
            {
                response = await SendRequest(request, authToken);
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
            var request = new RestRequest($"{_secondPartUrl}{requestString}", method);
            request.AddJsonBody(requestDto);
            RestResponse response;
            try
            {
                response = await SendRequest(request, authToken);
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

            var response = await _client.ExecuteAsync(request);
            return response;
        }
    }
}
