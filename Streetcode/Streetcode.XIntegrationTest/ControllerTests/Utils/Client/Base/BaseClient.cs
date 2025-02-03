using RestSharp;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base
{
    public class BaseClient
    {
        private readonly RestClient client;

        public BaseClient(HttpClient client, string secondPartUrl = "")
        {
            this.client = new RestClient(client) { AcceptedContentTypes = ContentType.JsonAccept };
            SecondPartUrl = secondPartUrl;
        }

        protected string SecondPartUrl { get; }

        protected async Task<RestResponse> SendQuery(string requestString, string authToken = "")
        {
            var request = new RestRequest($"{SecondPartUrl}{requestString}");
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

        protected async Task<RestResponse> SendCommand<T>(string requestString, Method method, T? requestDto = default, string authToken = "")
            where T : class?
        {
            var request = new RestRequest($"{SecondPartUrl}{requestString}", method);
            if (requestDto is not null)
            {
                request.AddJsonBody(requestDto);
            }

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

        protected async Task<RestResponse> SendCommand(string requestString, Method method, string authToken = "")
        {
            return await SendCommand<object>(requestString, method, null, authToken);
        }

        private async Task<RestResponse> SendRequest(RestRequest request, string authToken)
        {
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddHeader("Authorization", $"Bearer {authToken}");
            request.AddHeader("Content-Type", "application/json");

            var response = await client.ExecuteAsync(request);
            return response;
        }
    }
}
