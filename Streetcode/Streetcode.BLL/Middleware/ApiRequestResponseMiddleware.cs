using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.BLL.Services.Logging;

namespace Streetcode.BLL.Middleware
{
    public class ApiRequestResponseMiddleware : IMiddleware
    {
        private readonly ILoggerService _loggerService;
        private readonly RequestResponseMiddlewareOptions _options;

        public ApiRequestResponseMiddleware(ILoggerService loggerService, IOptions<RequestResponseMiddlewareOptions> options)
        {
            _loggerService = loggerService;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = await GetRequestAsTextAsync(context.Request);
            var requestTemplate = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}{Environment.NewLine}Request body: {request}{Environment.NewLine}";
            try
            {
                var response = await FormatResponseAsync(context, next);
                _loggerService.LogInformation($"{requestTemplate}Response body: {response}");
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"{requestTemplate}An unhandled exception was thrown by the application: {exception}");
            }
        }

        private async Task<string> FormatResponseAsync(HttpContext context, RequestDelegate next)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next(context);

            var response = await GetResponseAsTextAsync(context.Response);
            var filteredResponse = GetFilteredBody(response);
            await responseBody.CopyToAsync(originalBodyStream);
            return filteredResponse;
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
            catch (Exception ex)
            {
                _loggerService.LogError($"Unexpected error occured in {MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name}. Tried to parse body: {body}. Exception: {ex}");
                return string.Empty;
            }
        }

        private void TruncateProperties(JToken token)
        {
            foreach (var property in token.Children<JProperty>().ToList())
            {
                if (_options.PropertiesToIgnore.Contains(property.Name.ToString().ToLower()))
                {
                    property.Remove();
                    continue;
                }

                if (!_options.PropertiesToShorten.Contains(property.Name.ToString().ToLower()))
                {
                    continue;
                }

                if (property.Value.Type == JTokenType.String && property.Value.ToString().Length > _options.MaxResponseLength)
                {
                    property.Value = new JValue(property.Value.ToString().Substring(0, _options.MaxResponseLength) + "...");
                }
                else if (property.Value.Type == JTokenType.Object || property.Value.Type == JTokenType.Array)
                {
                    TruncateProperties(property.Value);
                }
            }
        }
    }
}
