using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.Logging;

namespace Streetcode.BLL.Middleware;

public class ApiRequestResponseMiddleware<T> : IMiddleware
{
    private readonly ILoggerService<T> _loggerService;

    public ApiRequestResponseMiddleware(ILoggerService<T> loggerService)
    {
        _loggerService = loggerService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var originalBodyStream = context.Response.Body;

        var request = await GetRequestAsTextAsync(context.Request);

        _loggerService.LogInformation(request);

        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context);

        var response = await GetResponseAsTextAsync(context.Response);

        var filteredResponse = GetResponseWithMaxLegth(response);

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

    private async Task<string> GetResponseAsTextAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);

        var text = await new StreamReader(response.Body).ReadToEndAsync();

        response.Body.Seek(0, SeekOrigin.Begin);

        return text;
    }

    private string? GetResponseWithMaxLegth(string response)
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
        catch (Exception ex)
        {
            return response;
        }
    }

    private void TruncateProperties(JToken token)
    {
        foreach (var property in token.Children<JProperty>())
        {
            if (property.Value.Type == JTokenType.String && property.Value.ToString().Length > 100)
            {
                property.Value = new JValue(property.Value.ToString().Substring(0, 100));
            }
            else if (property.Value.Type == JTokenType.Object || property.Value.Type == JTokenType.Array)
            {
                TruncateProperties(property.Value);
            }
        }
    }
}