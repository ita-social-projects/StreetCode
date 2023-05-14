using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.BLL.Middleware;

public class ApiRequestResponseMiddleware : IMiddleware
{
    private readonly ILoggerService _loggerService;

    public ApiRequestResponseMiddleware(ILoggerService loggerService)
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
        JObject jsonObject = JObject.Parse(response);

        foreach (var property in jsonObject.Properties())
        {
            if (property.Value.ToString().Length > 100)
            {
                string currentValue = property.Value.ToString();

                property.Value = currentValue.Substring(0, 100);
            }
        }

        return jsonObject.ToString();
    }
}
