using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Streetcode.BLL.Middleware
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseMiddleware<T>(this IApplicationBuilder app)
            where T : IMiddleware
        {
            return app.UseMiddleware(typeof(T));
        }
    }
}