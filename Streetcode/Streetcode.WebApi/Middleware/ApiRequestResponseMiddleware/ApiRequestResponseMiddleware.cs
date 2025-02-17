using System.Reflection;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Streetcode.WebApi.Middleware.ApiRequestResponseMiddleware
{
    public class ApiRequestResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiRequestResponseMiddleware> _loggerService;
        private readonly RequestResponseMiddlewareOptions _options;

        public ApiRequestResponseMiddleware(RequestDelegate next, ILogger<ApiRequestResponseMiddleware> loggerService, IOptions<RequestResponseMiddlewareOptions> options)
        {
            _loggerService = loggerService;
            _options = options.Value;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = await GetRequestAsTextAsync(context.Request);
            var requestTemplate =
                $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}{Environment.NewLine}";
            requestTemplate += request.IsNullOrEmpty() ? "Request body is empty" : $"Request body: {request}";
            requestTemplate += $"{Environment.NewLine}";
            try
            {
                var response = await FormatResponseAsync(context, _next);
                _loggerService.LogInformation($"{requestTemplate}Response body: {response}");
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"{requestTemplate}An unhandled exception was thrown by the application: {exception}");
                throw;
            }
        }

        private async Task<string> FormatResponseAsync(HttpContext context, RequestDelegate next)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next(context);

            var response = await GetResponseAsTextAsync(context.Response);
            await responseBody.CopyToAsync(originalBodyStream);

            return response;
        }

        private async Task<string> GetRequestAsTextAsync(HttpRequest request)
        {
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();
            request.Body.Position = 0;

            var filteredBody = GetFilteredBody(requestBody);

            return filteredBody;
        }

        private async Task<string> GetResponseAsTextAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            var encoding = Encoding.UTF8;

            var text = await new StreamReader(response.Body, encoding).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            var filteredBody = GetFilteredBody(text);

            return filteredBody;
        }

        private string GetFilteredBody(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return string.Empty;
            }

            try
            {
                var jsonObject = JToken.Parse(body);

                if (jsonObject.Type == JTokenType.Array)
                {
                    var jsonArray = (JArray)jsonObject;

                    foreach (var item in jsonArray)
                    {
                        TruncateProperties(item);
                    }
                }
                else if (jsonObject.Type == JTokenType.Object)
                {
                    TruncateProperties(jsonObject);
                }

                return jsonObject.ToString();
            }
            catch (JsonReaderException)
            {
                return "Response body is in gzip format";
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Unexpected error occured in {MethodBase.GetCurrentMethod() !.DeclaringType!.Name}.{MethodBase.GetCurrentMethod() !.Name}. Tried to parse body: {body}. Exception: {ex}");
                return string.Empty;
            }
        }

        private void TruncateProperties(JToken token)
        {
            switch (token)
            {
                case JObject obj:
                {
                    foreach (var property in obj.Properties())
                    {
                        if (property.Value is JValue)
                        {
                            TruncateValue(property);
                        }
                        else
                        {
                            TruncateProperties(property.Value);
                        }
                    }

                    break;
                }

                case JArray array:
                {
                    foreach (var item in array)
                    {
                        TruncateProperties(item);
                    }

                    break;
                }
            }
        }

        private void TruncateValue(JProperty property)
        {
            if (_options.PropertiesToIgnore.Contains(property.Name.ToLower()))
            {
                property.Remove();
            }

            var valueAsString = property.Value.ToString();
            if (valueAsString.Length > _options.MaxResponseLength)
            {
                var shortenedLog = new JValue(valueAsString.Substring(0, _options.MaxResponseLength));
                property.Value = $"{shortenedLog}...";
            }
        }
    }
}
