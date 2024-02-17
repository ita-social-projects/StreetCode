using Microsoft.AspNetCore.ResponseCompression;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Middleware;

public class CustomResponseCompressionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ResponseCompressionMiddleware _responseCompressionMiddleware;

    public CustomResponseCompressionMiddleware(RequestDelegate next, IResponseCompressionProvider provider)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _responseCompressionMiddleware = new ResponseCompressionMiddleware(next, provider);
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<CompressResponseAttribute>() == null)
        {
            await _next(context);
        }
        else
        {
            await _responseCompressionMiddleware.Invoke(context);
        }
    }
}
