using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.Logging;

namespace Streetcode.BLL.Middleware
{
    public class ApiRequestResponseMiddleware : IMiddleware
    {
        private readonly ILoggerService _loggerService;
        private readonly string[] _loggerPropertiesExceptions;
        private readonly string[] _exceptionProperties;
        private readonly int _maxResponseLength;

        public ApiRequestResponseMiddleware(ILoggerService loggerService, IConfiguration configuration)
        {
            _loggerService = loggerService;
            _maxResponseLength = configuration.GetValue<int>("Middleware:ApiRequestResponse:MaxResponseLength");
            _exceptionProperties = configuration.GetSection("Middleware:ApiRequestResponse:ExceptionProperties")
                .Get<string[]>()
                .Select(p => p.ToLower())
                .ToArray();
            _loggerPropertiesExceptions = configuration.GetSection("Serilog:PropertiesToIgnore")
                .Get<string[]>()
                .Select(p => p.ToLower())
                .ToArray();
            //_loggerPropertiesExceptions = configuration.GetSection("Serilog:PropertiesToIgnore").Get<string[]>();
            //_exceptionProperties = configuration.GetSection("Middleware:ApiRequestResponse:ExceptionProperties").Get<string[]>();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var originalBodyStream = context.Response.Body;

            var request = await GetRequestAsTextAsync(context.Request);
            LogRequest(request);

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next(context);

            var response = await GetResponseAsTextAsync(context.Response);
            var filteredResponse = GetFilteredResponse(response);
            LogResponse(filteredResponse);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private void LogRequest(string request)
        {
            _loggerService.LogInformation(request);
        }

        private void LogResponse(string response)
        {
            _loggerService.LogInformation(response);
        }

        private async Task<string> GetRequestAsTextAsync(HttpRequest request)
        {
            var requestBody = request.Body;

            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body = requestBody;

            return $"{request.Scheme} {request.Host} {request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> GetResponseAsTextAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            var encoding = Encoding.UTF8;

            var text = await new StreamReader(response.Body, encoding).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }

        private string GetFilteredResponse(string response)
        {
            try
            {
                var jsonObject = JToken.Parse(response);

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
            catch (Exception)
            {
                return null;
            }
        }

        private void TruncateProperties(JToken token)
        {
            foreach (var property in token.Children<JProperty>().ToList())
            {
                if (!_loggerPropertiesExceptions.Contains(property.Name.ToString().ToLower()))
                {
                    if (_exceptionProperties.Contains(property.Name.ToString().ToLower()))
                    {
                        continue;
                    }

                    if (property.Value.Type == JTokenType.String && property.Value.ToString().Length > _maxResponseLength)
                    {
                        property.Value = new JValue(property.Value.ToString().Substring(0, _maxResponseLength));
                    }
                    else if (property.Value.Type == JTokenType.Object || property.Value.Type == JTokenType.Array)
                    {
                        TruncateProperties(property.Value);
                    }
                }
                else
                {
                    property.Remove();
                }
            }
        }
    }
}
