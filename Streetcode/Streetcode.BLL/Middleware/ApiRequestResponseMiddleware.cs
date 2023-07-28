using System;
using System.IO;
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
        private readonly MiddlewareOptions _options;

        public ApiRequestResponseMiddleware(ILoggerService loggerService, IOptions<MiddlewareOptions> options)
        {
            _loggerService = loggerService;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await LogRequestAsync(context);
            await LogResponseAsync(context, next);
        }

        private async Task LogRequestAsync(HttpContext context)
        {
            var request = await GetRequestAsTextAsync(context.Request);
            _loggerService.LogInformation(request);
        }

        private async Task LogResponseAsync(HttpContext context, RequestDelegate next)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next(context);

            var response = await GetResponseAsTextAsync(context.Response);
            var filteredResponse = GetFilteredResponse(response);
            _loggerService.LogInformation(filteredResponse);
            await responseBody.CopyToAsync(originalBodyStream);
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

        private static async Task<string> GetResponseAsTextAsync(HttpResponse response)
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
                if (!_options.PropertiesToIgnore.Contains(property.Name.ToString().ToLower()))
                {
                    if (!_options.PropertiesToShorten.Contains(property.Name.ToString().ToLower()))
                    {
                        continue;
                    }

                    if (property.Value.Type == JTokenType.String && property.Value.ToString().Length > _options.MaxResponseLength)
                    {
                        property.Value = new JValue(property.Value.ToString().Substring(0, _options.MaxResponseLength));
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
