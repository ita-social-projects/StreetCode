using Microsoft.AspNetCore.Builder;

namespace Streetcode.BLL.Middleware
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseRequestResponseMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiRequestResponseMiddleware>();
        }
    }
}
