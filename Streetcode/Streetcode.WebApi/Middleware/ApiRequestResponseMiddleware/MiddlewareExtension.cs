namespace Streetcode.WebApi.Middleware.ApiRequestResponseMiddleware
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseRequestResponseMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiRequestResponseMiddleware>();
        }
    }
}
